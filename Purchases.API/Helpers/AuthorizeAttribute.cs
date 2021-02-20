using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Purchases.API.Models.DTOs;
using System;

namespace Purchases.API.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!(context.HttpContext.Items["User"] is UserDTO))
                context.Result = new JsonResult(new { message = "Unauthorized"})
                    { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
