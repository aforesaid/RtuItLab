using Shops.Domain.Services;

namespace RabbitMQ.Consumers.Shops
{
    public class ShopsBaseConsumer
    {
        protected readonly IShopsService ShopsService;
        public ShopsBaseConsumer(IShopsService shopsService)
        {
            ShopsService = shopsService;
        }
    }
}
