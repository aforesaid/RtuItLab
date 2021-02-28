using Identity.Domain.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using RtuItLab.Infrastructure.Models.Identity;
using System.Threading.Tasks;

namespace RabbitMQ.Consumers.Identity
{
    public class Authenticate : IdentityBaseConsumer, IConsumer<LoginRequest>
    {
        private readonly ILogger<Authenticate> _logger;

        public Authenticate(IUserService userService, ILogger<Authenticate> logger) : base(userService)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<LoginRequest> context)
        {
            _logger.LogInformation($"Value: {context.Message.Password} {context.Message.Username}");
            var order = await UserService.Authenticate(context.Message);
            await context.RespondAsync(order);
        }
    }
}
