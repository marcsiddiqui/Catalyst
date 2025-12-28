using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Stores;
using Nop.Core.Infrastructure;
using Nop.Services.Common;
using Nop.Core;
using System.Globalization;
using System.Text.Json;
using Newtonsoft.Json;
using Nop.Core.Domain.ApiModels;
using Nop.Services.Customers;
using Nop.Services.Logging;
using Microsoft.AspNetCore.Http.Extensions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeApiAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (Customer)context.HttpContext.Items["User"];
        if (user == null)
        {
            // not logged in
            context.Result = new JsonResult(new { Message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}