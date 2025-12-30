using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using Nop.Web.Framework.Middleware;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Nop.Web
{
    public class JwtService
    {
        private readonly string _issuer;
        private readonly string _secret;
        private readonly string _expDate;

        public JwtService(IConfiguration config)
        {
            _issuer = config.GetSection("Jwt").GetSection("Issuer").Value;
            _secret = config.GetSection("Jwt").GetSection("Key").Value;
            _expDate = config.GetSection("Jwt").GetSection("expirationInMinutes").Value;
        }

        public (string, DateTime) GenerateSecurityToken(string email, int customerId, Guid sessionGuid, bool isPersist = false)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtValueType.CustomerEmail, email));
            permClaims.Add(new Claim(JwtValueType.CustomerId, customerId.ToString()));
            permClaims.Add(new Claim(JwtValueType.SessionId, sessionGuid.ToString()));

            var expireTime = !isPersist ? DateTime.UtcNow.AddDays(1) : DateTime.UtcNow.AddMonths(1);

            var token = new JwtSecurityToken(
                _issuer,
                _issuer,
                claims: permClaims,
                expires: expireTime,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expireTime);

        }
    }
}