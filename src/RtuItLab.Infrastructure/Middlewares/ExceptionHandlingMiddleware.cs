using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RtuItLab.Infrastructure.Exceptions;
using RtuItLab.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace RtuItLab.Infrastructure.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex.Message);
            var value = ex is BadRequestException;
            var code = ex switch
            {
                NotFoundException _ => HttpStatusCode.NotFound,
                BadRequestException _ => HttpStatusCode.BadRequest,
                ForbiddenException _ => HttpStatusCode.Forbidden,
                _ => HttpStatusCode.InternalServerError
            };

            var errors = new List<string> { ex.Message };

            var result = JsonSerializer.Serialize(ApiResult<string>.Failure((int)code, errors));

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
