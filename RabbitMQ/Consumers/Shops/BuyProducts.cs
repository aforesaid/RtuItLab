using System.Threading.Tasks;
using MassTransit;
using RtuItLab.Infrastructure.MassTransit.Requests.Shops;
using Shops.Domain.Services;

namespace RabbitMQ.Consumers.Shops
{
    public class BuyProducts : ShopsBaseConsumer, IConsumer<BuyProductsRequest>
    {
        public BuyProducts(IShopsService shopsService) : base(shopsService)
        {
        }

        public async Task Consume(ConsumeContext<BuyProductsRequest> context)
        {
            var order = await ShopsService.BuyProducts(context.Message.ShopId, context.Message.Products);
            await context.RespondAsync(order);
        }
    }
}
