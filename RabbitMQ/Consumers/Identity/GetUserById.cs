using System.Threading.Tasks;
using Identity.Domain.Services;
using MassTransit;

namespace RabbitMQ.Consumers.Identity
{
    public class GetUserById : IdentityBaseConsumer, IConsumer<string>
    {
        public GetUserById(IUserService userService) : base(userService)
        {
        }
        public async Task Consume(ConsumeContext<string> context)
        {
            var order = await UserService.GetUserById(context.Message);
            await context.RespondAsync(order);
        }
    }
}
