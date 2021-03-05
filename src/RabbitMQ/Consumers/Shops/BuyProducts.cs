using MassTransit;
using Purchases.Domain.Services;
using RtuItLab.Infrastructure.MassTransit.Shops.Requests;
using RtuItLab.Infrastructure.MassTransit.Shops.Responses;
using Shops.Domain.Services;
using System.Threading.Tasks;

namespace RabbitMQ.Consumers.Shops
{
    public class BuyProducts : ShopsBaseConsumer, IConsumer<BuyProductsRequest>
    {
        private readonly IPurchasesService _purchasesService;
        public BuyProducts(IShopsService shopsService,
            IPurchasesService purchasesService) : base(shopsService)
        {
            _purchasesService = purchasesService;
        }

        public async Task Consume(ConsumeContext<BuyProductsRequest> context)
        {
            var order = await ShopsService.BuyProducts(context.Message.ShopId, context.Message.Products);
            await context.RespondAsync(order);
            if (order.Exception is null)
            {
                var transaction =
                    await ShopsService.CreateTransaction(context.Message.ShopId, context.Message.Products);
                await _purchasesService.AddTransaction(context.Message.User, transaction);
            }
        }
    }
}
