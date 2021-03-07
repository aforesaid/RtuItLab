using Shops.Domain.Services;

namespace Shops.API.Consumers
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
