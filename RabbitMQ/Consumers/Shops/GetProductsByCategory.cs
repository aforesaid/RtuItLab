using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using RtuItLab.Infrastructure.MassTransit.Requests.Shops;
using Shops.Domain.Services;

namespace RabbitMQ.Consumers.Shops
{
    public class GetProductsByCategory : ShopsBaseConsumer, IConsumer<GetProductsByCategoryRequest>
    {
        public GetProductsByCategory( IShopsService shopsService) : base(shopsService)
        {
        }

        public async Task Consume(ConsumeContext<GetProductsByCategoryRequest> context)
        {
            var order = await ShopsService.GetProductsByCategory(context.Message.ShopId, context.Message.Category);
            await context.RespondAsync(new GetProductsResponse{
            Success = order != null,
            Products = order?.ToList()});
        }
    }
}
