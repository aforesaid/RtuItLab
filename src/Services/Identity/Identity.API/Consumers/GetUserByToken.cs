using System.Threading.Tasks;
using Identity.Domain.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using RtuItLab.Infrastructure.Models.Identity;

namespace Identity.API.Consumers
{
    public class GetUserByToken : IdentityBaseConsumer<GetUserByToken>, IConsumer<TokenRequest>
    {
        public GetUserByToken(IUserService userService,
            ILogger<GetUserByToken> logger) : base(userService, logger)
        {
        }

        public async Task Consume(ConsumeContext<TokenRequest> context)
        {
            var order = await UserService.GetUserByToken(context.Message);
            await context.RespondAsync(order);
        }
    }
}