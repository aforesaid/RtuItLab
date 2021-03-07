using System.Threading.Tasks;
using MassTransit;
using RtuItLab.Infrastructure.MassTransit.Shops.Requests;
using Shops.Domain.Services;

namespace Shops.API.Consumers
{
    public class GetProductsByCategory : ShopsBaseConsumer, IConsumer<GetProductsByCategoryRequest>
    {
        public GetProductsByCategory( IShopsService shopsService) : base(shopsService)
        {
        }

        public async Task Consume(ConsumeContext<GetProductsByCategoryRequest> context)
        {
            var order = await ShopsService.GetProductsByCategory(context.Message.ShopId, context.Message.Category);
            await context.RespondAsync(order);
        }
    }
}
