using MassTransit;
using Microsoft.AspNetCore.Http;
using RtuItLab.Infrastructure.MassTransit;
using RtuItLab.Infrastructure.Models.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RtuItLab.Infrastructure.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Uri _rabbitMqUri = new Uri("rabbitmq://localhost/identityQueue");
        public JwtMiddleware(RequestDelegate next)
        {
            _next        = next;
        }

        public async Task Invoke(HttpContext context, IBusControl busControl)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split().Last();
            if (token != null)
                await AttachUserToContext(context, busControl, token);
            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, IBusControl busControl, string token)
        {
            var client = busControl.CreateRequestClient<TokenRequest>(_rabbitMqUri);
            var response = await client.GetResponse<ResponseMassTransit<User>>(new TokenRequest
            {
                Token = token
            });
            if (response.Message.Content != null)
                context.Items["User"] = response.Message.Content;
        }
    }
}