using System.Threading.Tasks;
using Identity.Domain.Services;
using MassTransit;
using ServicesDtoModels.Models.Identity;

namespace RabbitMQ.Consumers.Identity
{
    public class Authenticate : IdentityBaseConsumer, IConsumer<AuthenticateRequest>
    {
        public Authenticate(IUserService userService) : base(userService)
        {
        }

        public async Task Consume(ConsumeContext<AuthenticateRequest> context)
        {
            var order = await UserService.Authenticate(context.Message);
            await context.RespondAsync(order);
        }
    }
}
