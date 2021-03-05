using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using RtuItLab.Infrastructure.MassTransit.Shops.Requests;
using RtuItLab.Infrastructure.MassTransit.Shops.Responses;
using Shops.Domain.Services;

namespace RabbitMQ.Consumers.Shops
{
    public class GetAllShops : ShopsBaseConsumer, IConsumer<GetAllShopsRequest>
    {
        public GetAllShops(IShopsService shopsService) : base(shopsService)
        {
        }
        public async Task Consume(ConsumeContext<GetAllShopsRequest> context)
        {
            var order = ShopsService.GetAllShops();
            await context.RespondAsync(order);
        }
    }
}
