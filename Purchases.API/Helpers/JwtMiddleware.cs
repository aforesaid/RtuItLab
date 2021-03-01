using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using RtuItLab.Infrastructure.Models.Identity;
using ServicesDtoModels.Models.Identity;

namespace Purchases.API.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IBusControl busControl)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
            if (token != null)
                await AttachUserToContext(context, busControl, token);
            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, IBusControl busControl, string token)
        {
            var serviceAddress = new Uri("rabbitmq://localhost/identityQueue");
            var client = busControl.CreateRequestClient<TokenRequest>(serviceAddress);
            var response = await client.GetResponse<User>(new TokenRequest
            {
                Token = token
            });
            var user = response.Message;
            if (user.Id != null)
            {
                context.Items["User"] = response.Message;
            }
        }
    }
}
