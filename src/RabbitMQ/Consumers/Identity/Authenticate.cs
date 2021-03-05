using Identity.Domain.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using RtuItLab.Infrastructure.Models.Identity;
using System.Threading.Tasks;

namespace RabbitMQ.Consumers.Identity
{
    public class Authenticate : IdentityBaseConsumer<Authenticate>, IConsumer<AuthenticateRequest>
    {
        public Authenticate(IUserService userService,
            ILogger<Authenticate> logger) : base(userService, logger)
        {
        }

        public async Task Consume(ConsumeContext<AuthenticateRequest> context)
        {
            Logger.LogInformation($"Value: {context.Message.Password} {context.Message.Username}");
            var order = await UserService.Authenticate(context.Message);
            await context.RespondAsync(order);
        }
    }
}
