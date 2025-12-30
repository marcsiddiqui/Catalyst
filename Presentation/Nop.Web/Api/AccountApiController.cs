using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

using Nop.Core;
using Nop.Core.Domain;
using Nop.Core.Domain.ApiModels;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Events;
using Nop.Core.Http.Extensions;
using Nop.Services.Authentication;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Web.Factories;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Models.Customer;

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nop.Web.Api
{
    [ApiExplorerSettings(GroupName = SwaggerAPIGroup.Public)]
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountApiController : Controller
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IStoreContext _storeContext;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IWebHelper _webHelper;
        private readonly Nop.Services.Logging.ILogger _logger;
        private readonly CustomerSettings _customerSettings;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ILanguageService _languageService;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        public AccountApiController(
            ILocalizationService localizationService,
            IStoreContext storeContext,
            ICustomerService customerService,
            IWorkContext workContext,
            IShoppingCartService shoppingCartService,
            ICustomerActivityService customerActivityService,
            IWebHelper webHelper,
            Nop.Services.Logging.ILogger logger,
            CustomerSettings customerSettings,
            IAuthenticationService authenticationService,
            IEventPublisher eventPublisher,
            ICustomerRegistrationService customerRegistrationService,
            ILanguageService languageService,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor)
        {
            _localizationService = localizationService;
            _storeContext = storeContext;
            _customerService = customerService;
            _workContext = workContext;
            _shoppingCartService = shoppingCartService;
            _customerActivityService = customerActivityService;
            _webHelper = webHelper;
            _logger = logger;
            _customerSettings = customerSettings;
            _authenticationService = authenticationService;
            _eventPublisher = eventPublisher;
            _customerRegistrationService = customerRegistrationService;
            _languageService = languageService;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }


        #endregion

        #region Methods

        /// <summary>
        /// This action is used for login
        /// </summary>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        [HttpPost("login")]
        public virtual async Task<IActionResult> CustomerLogin(CustomerLoginModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest(new { success = false, Message = await _localizationService.GetResourceAsync("Account.Login.Enter.Email"), token = "" });
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { success = false, Message = await _localizationService.GetResourceAsync("Account.Login.Enter.Password"), token = "" });
            }
            var customerUserName = model.Email?.Trim();
            var customerEmail = model.Email?.Trim();
            var userNameOrEmail = _customerSettings.UsernamesEnabled ? customerUserName : customerEmail;

            var loginResult = await _customerRegistrationService.ValidateCustomerAsync(userNameOrEmail, model.Password);
            switch (loginResult)
            {
                case CustomerLoginResults.Successful:
                    {
                        var customer = _customerSettings.UsernamesEnabled
                            ? await _customerService.GetCustomerByUsernameAsync(customerUserName)
                            : await _customerService.GetCustomerByEmailAsync(customerEmail);

                        var jwt = new JwtService(_config);

                        Guid SessionGuid = Guid.NewGuid();

                        var (token, expireTime) = jwt.GenerateSecurityToken(customer.Email, customer.Id, SessionGuid, model.RememberMe);

                        //Migrate Shopping Cart
                        await _shoppingCartService.MigrateShoppingCartAsync(customer, customer, true);

                        //Set Current Customer
                        await _workContext.SetCurrentCustomerAsync(customer, SessionGuid);

                        //Sign In New Customer
                        await _authenticationService.SignInAsync(customer, SessionGuid, false);

                        string deviceName = "";
                        string deviceToken = "";
                        string deviceTimeZone = "";
                        bool isMobileAppLogin = false;

                        if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Request != null && _httpContextAccessor.HttpContext.Request.Headers != null)
                        {
                            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey(HeaderNames.UserAgent))
                                deviceName = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent];

                            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey(ApiHeader.ExpoToken))
                                deviceToken = _httpContextAccessor.HttpContext.Request.Headers[ApiHeader.ExpoToken];

                            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey(ApiHeader.TimeZone))
                                deviceTimeZone = _httpContextAccessor.HttpContext.Request.Headers[ApiHeader.TimeZone];

                            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey(ApiHeader.IsRequestFromApp))
                                isMobileAppLogin = _httpContextAccessor.HttpContext.Request.Headers[ApiHeader.IsRequestFromApp] == "true";
                        }

                        await _customerService.InsertCustomerSessionAsync(new CustomerSession
                        {
                            CustomerId = customer.Id,
                            CreatedOnUtc = DateTime.UtcNow,
                            ExpiresOnUtc = expireTime,
                            IsActive = true,
                            SessionId = SessionGuid,
                            DeviceVersion = deviceName,
                            DeviceToken = deviceToken,
                            TimeZone = deviceTimeZone,
                            LoginType = isMobileAppLogin ? LoginType.MobileApp : LoginType.PublicApi
                        });

                        //Publish Event       
                        await _eventPublisher.PublishAsync(new CustomerLoggedinEvent(customer));

                        //Activity Log
                        await _customerActivityService.InsertActivityAsync(customer, "PublicStore.Login",
                            await _localizationService.GetResourceAsync("ActivityLog.PublicStore.Login"), customer);

                        return Ok(new SuccessAuthenticatedApiResponseModel(await _localizationService.GetResourceAsync("Account.Login.Successfully"))
                        {
                            Token = token,
                            TokenType = "Bearer"
                        });
                    }
                case CustomerLoginResults.MultiFactorAuthenticationRequired:
                    {
                        var customerMultiFactorAuthenticationInfo = new CustomerMultiFactorAuthenticationInfo
                        {
                            UserName = userNameOrEmail,
                            RememberMe = false,
                            ReturnUrl = ""
                        };
                        // HttpContext.Session.Set(NopCustomerDefaults.CustomerMultiFactorAuthenticationInfo, customerMultiFactorAuthenticationInfo);
                        //#MH: Not sure about replacement for this at the moment
                        return RedirectToRoute("MultiFactorVerification");
                    }
                case CustomerLoginResults.CustomerNotExist:
                    return BadRequest(new { Success = false, Message = await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.CustomerNotExist") });
                case CustomerLoginResults.Deleted:
                    return BadRequest(new { Success = false, Message = await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.Deleted") });
                case CustomerLoginResults.NotActive:
                    return BadRequest(new { Success = false, Message = await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.NotActive") });
                case CustomerLoginResults.NotRegistered:
                    return BadRequest(new { Success = false, Message = await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.NotRegistered") });
                case CustomerLoginResults.LockedOut:
                    return BadRequest(new { Success = false, Message = await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.LockedOut") });
                case CustomerLoginResults.WrongPassword:
                default:
                    return BadRequest(new { Success = false, Message = await _localizationService.GetResourceAsync("Account.Login.WrongCredentials") });
            }
        }

        /// <summary>
        /// This action is used for logout
        /// </summary>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AuthorizeApi]
        [HttpPost("logout")]
        public virtual async Task<IActionResult> CustomerLogout()
        {
            var currentCustomer = await _workContext.GetCurrentCustomerAsync();
            //activity log
            await _customerActivityService.InsertActivityAsync(currentCustomer, "PublicStore.Logout", await _localizationService.GetResourceAsync("ActivityLog.PublicStore.Logout"), currentCustomer);

            //standard logout 
            await _authenticationService.SignOutAsync();

            //raise logged out event       
            await _eventPublisher.PublishAsync(new CustomerLoggedOutEvent(currentCustomer));

            return Ok(new { Success = true });
        }

        /// <summary>
        /// This action is used for logs to server
        /// </summary>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AuthorizeApi]
        [HttpPost("log")]
        public virtual async Task<IActionResult> Log()
        {
            using StreamReader reader = new(Request.Body);
            var bodyAsString = await reader.ReadToEndAsync();

            await _logger.InsertLogAsync(Core.Domain.Logging.LogLevel.Information, bodyAsString);

            return Ok(new { Success = true });
        }

        #endregion

        #region Models

        public class CustomerLoginModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        #endregion
    }
}
