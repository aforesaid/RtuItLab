using System.Threading.Tasks;
using MassTransit;
using Shops.Domain.Services;

namespace RabbitMQ.Consumers.Shops
{
    public class GetProductsByShop : ShopsBaseConsumer, IConsumer<string>
    {
        public GetProductsByShop(IShopsService shopsService) : base(shopsService)
        {
            
        }

        public async Task Consume(ConsumeContext<string> context)
        {
            var isParse = int.TryParse(context.Message, out var shopId);
            if (isParse)
            {
                var order = await ShopsService.GetProductsByShop(shopId);
                await context.RespondAsync(order);
            }
        }
    }
}
