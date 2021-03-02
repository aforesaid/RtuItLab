using System;
using MassTransit;
using Microsoft.Extensions.Logging;
using RtuItLab.Infrastructure.MassTransit.Requests.Shops;
using Shops.Domain.Services;
using System.Threading.Tasks;
using Purchases.Domain.Services;

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
            var (message,success) = await ShopsService.BuyProducts(context.Message.ShopId, context.Message.Products);
            await context.RespondAsync(new BuyProductsResponse{
            Success = success,
            Message = message
            });
            if (success)
            {
                var transaction =
                    await ShopsService.CreateTransaction(context.Message.ShopId, context.Message.Products);
                await _purchasesService.AddTransaction(context.Message.User, transaction);
            }
        }
    }
}
