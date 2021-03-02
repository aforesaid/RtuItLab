using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using RtuItLab.Infrastructure.MassTransit.Shops.Requests;
using RtuItLab.Infrastructure.MassTransit.Shops.Responses;
using Shops.Domain.Services;

namespace RabbitMQ.Consumers.Shops
{
    //TODO: исправить, если можно, так как параметры к методу не нужны
    public class GetAllShops : ShopsBaseConsumer, IConsumer<GetAllShopsRequest>
    {
        public GetAllShops(IShopsService shopsService) : base(shopsService)
        {
        }
        public async Task Consume(ConsumeContext<GetAllShopsRequest> context)
        {
            var order = ShopsService.GetAllShops();
            await context.RespondAsync(new GetAllShopsResponse{
                Shops = order?.ToList(),
                Success = order != null
            });
        }
    }
}
