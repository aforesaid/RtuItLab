using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Domain.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using RtuItLab.Infrastructure.Models.Identity;

namespace RabbitMQ.Consumers.Identity
{
    public class CreateUser : IdentityBaseConsumer, IConsumer<AuthenticateRequest>
    {
        private readonly ILogger<CreateUser> _logger;
        public CreateUser(IUserService userService, ILogger<CreateUser> logger) : base(userService)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AuthenticateRequest> context)
        {
            _logger.LogInformation($"Value: {context.Message.Password} {context.Message.Username}");
            var order = await UserService.CreateUser(context.Message);
            _logger.LogInformation($"Count error: {order.Errors.Count()}");
            await context.RespondAsync(order);
        }
    }
}
