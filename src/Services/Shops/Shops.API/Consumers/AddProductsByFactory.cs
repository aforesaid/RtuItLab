using System.Threading.Tasks;
using MassTransit;
using RtuItLab.Infrastructure.MassTransit.Shops.Requests;
using Shops.Domain.Services;

namespace Shops.API.Consumers
{
    public class AddProductsByFactory : ShopsBaseConsumer, IConsumer<AddProductsByFactoryRequest>
    {
        public AddProductsByFactory(IShopsService shopsService) : base(shopsService)
        {
        }

        public async Task Consume(ConsumeContext<AddProductsByFactoryRequest> context)
        {
            await ShopsService.AddProductsByFactory(context.Message.Products);
        }
    }
}
