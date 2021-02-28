using System.Threading.Tasks;
using Identity.Domain.Services;
using MassTransit;
using RtuItLab.Infrastructure.Models.Identity;

namespace RabbitMQ.Consumers.Identity
{
    public class GetUserByToken : IdentityBaseConsumer, IConsumer<TokenRequest>
    {
        public GetUserByToken(IUserService userService) : base(userService)
        {
        }

        public async Task Consume(ConsumeContext<TokenRequest> context)
        {
            var order = await UserService.GetUserByToken(context.Message);
            await context.RespondAsync(order);
        }
    }
}
