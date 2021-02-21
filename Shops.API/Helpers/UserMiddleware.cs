using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shops.API.Helpers
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
            if (token != null)
                await AttachUserToContext(context, token);
            await _next(context);
        }
        private async Task AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                //TODO: добавить кролика для связи с Identity (дешифровка токена в контекст)
            }
            catch (Exception e)
            {
                //TODO: Add logging
            }
        }
    }
}
