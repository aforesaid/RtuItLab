using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RtuItLab.Infrastructure.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RtuItLab.Infrastructure.Filters
{
    public class ValidateModelAttribute : Attribute, IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(modelState => modelState.Errors)
                    .Select(modelError => modelError.ErrorMessage).ToList();

                context.Result = new BadRequestObjectResult(ApiResult<string>.Failure(400, errors));
            }
            await next();
        }
    }
}
