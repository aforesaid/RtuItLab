using Identity.Domain.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using RtuItLab.Infrastructure.Models.Identity;
using System.Threading.Tasks;

namespace RabbitMQ.Consumers.Identity
{
    public class CreateUser : IdentityBaseConsumer<CreateUser>, IConsumer<RegisterRequest>
    { 
        public CreateUser(IUserService userService,
            ILogger<CreateUser> logger) : base(userService, logger)
        {
        }

        public async Task Consume(ConsumeContext<RegisterRequest> context)
        {
            Logger.LogInformation($"Value: {context.Message.Password} {context.Message.Username}");
            var order = await UserService.CreateUser(context.Message);
            await context.RespondAsync(order);
        }
    }
}
