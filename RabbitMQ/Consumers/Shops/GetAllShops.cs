using System.Threading.Tasks;
using MassTransit;
using Shops.Domain.Services;

namespace RabbitMQ.Consumers.Shops
{
    //TODO: исправить, если можно, так как параметры к методу не нужны
    public class GetAllShops : ShopsBaseConsumer, IConsumer<string>
    {
        public GetAllShops(IShopsService shopsService) : base(shopsService)
        {
        }
        public async Task Consume(ConsumeContext<string> context)
        {
            var order = ShopsService.GetAllShops();
            await context.RespondAsync(order);
        }
    }
}
