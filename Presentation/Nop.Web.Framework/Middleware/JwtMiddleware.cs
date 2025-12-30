using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Customers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Web.Framework.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<JwtSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, ICustomerService userService, IWorkContext workContext)
        {
            var FullToken = context.Request.Headers["Authorization"].FirstOrDefault();

            var JwtToken = !string.IsNullOrWhiteSpace(FullToken) && FullToken.StartsWith("Bearer ") ? FullToken.Replace("Bearer ", "") : null;

            if (JwtToken != null)
                await AttachUserToContext(context, userService, workContext, JwtToken);

            await _next(context);
        }

        public async Task AttachUserToContext(HttpContext context, ICustomerService userService, IWorkContext workContext, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Key);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var customerIdString = jwtToken.GetValueFromJwtToken(JwtValueType.CustomerId);
                var customerEmail = jwtToken.GetValueFromJwtToken(JwtValueType.CustomerEmail);
                var sessionIdString = jwtToken.GetValueFromJwtToken(JwtValueType.SessionId);
                bool parseSuccess = true;
                int customerId = 0;
                Guid sessionId = default;
                parseSuccess = parseSuccess && int.TryParse(customerIdString, out customerId);
                parseSuccess = parseSuccess && Guid.TryParse(sessionIdString, out sessionId);

                // attach user to context on successful jwt validation
                var customer = await userService.GetCustomerByIdAsync(customerId);

                if (parseSuccess && customer != null && customer.Active && !customer.Deleted && !string.IsNullOrWhiteSpace(customer.Email) && customer.Email.Equals(customerEmail))
                {
                    var customerSession = await userService.GetAllCustomerSessionAsync(customerId: customer.Id);

                    if (!(
                        customerSession != null &&
                        customerSession.Any(x => x.SessionId.Equals(sessionId) && x.IsActive && (!x.ExpiresOnUtc.HasValue || (x.ExpiresOnUtc.HasValue && DateTime.UtcNow < x.ExpiresOnUtc.Value)))
                        ))
                        customer = null;

                    context.Items["User"] = customer;
                    await workContext.SetCurrentCustomerAsync(customer, sessionId);

                    var activeSession = customerSession != null ? customerSession.FirstOrDefault(x => x.SessionId.Equals(sessionId)) : null;

                    if (activeSession != null)
                    {
                        activeSession.LastActivityOnUtc = DateTime.UtcNow;

                        await userService.UpdateCustomerSessionAsync(activeSession);
                    }
                }
                //else
                //{
                //    context.Items["User"] = null;
                //    await workContext.SetCurrentCustomerAsync(null);
                //}
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }

       
    }

    public static class JwtExtensions
    {
        public static string GetValueFromJwtToken(this JwtSecurityToken jwtSecurityToken, string Type)
        {
            if (jwtSecurityToken == null || string.IsNullOrWhiteSpace(Type)) return null;

            return jwtSecurityToken.Claims.First(x => x.Type == Type).Value;
        }
    }

    public static class JwtValueType
    {
        public const string CustomerId = "UserId";
        public const string CustomerEmail = "Email";
        public const string SessionId = "SessionId";
    }
}
