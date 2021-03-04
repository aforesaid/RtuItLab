using System;
using Microsoft.AspNetCore.Mvc.Filters;
using RtuItLab.Infrastructure.Exceptions;
using RtuItLab.Infrastructure.Models.Identity;

namespace RtuItLab.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!(context.HttpContext.Items["User"] is User))
                throw new ForbiddenException("User unauthorized!");
        }
    }
}