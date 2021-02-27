using Identity.Domain.Services;

namespace RabbitMQ.Consumers.Identity
{
    public class IdentityBaseConsumer
    {
        protected readonly IUserService UserService;
        public IdentityBaseConsumer(IUserService userService)
        {
            UserService = userService;
        }
    }
}
