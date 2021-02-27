using MassTransit;
using ServicesDtoModels.Models.Shops;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shops.Domain.Services;

namespace RabbitMQ.Consumers.Shops
{
    public class AddProductsByFactory : ShopsBaseConsumer, IConsumer<ICollection<ProductByFactory>>
    {
        public AddProductsByFactory(IShopsService shopsService) : base(shopsService)
        {
        }

        public async Task Consume(ConsumeContext<ICollection<ProductByFactory>> context)
        {
            await ShopsService.AddProductsByFactory(context.Message);
            await context.RespondAsync(true);
        }
    }
}
