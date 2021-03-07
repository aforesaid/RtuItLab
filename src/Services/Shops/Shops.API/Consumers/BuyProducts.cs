using System;
using System.Threading.Tasks;
using MassTransit;
using RtuItLab.Infrastructure.MassTransit.Purchases.Requests;
using RtuItLab.Infrastructure.MassTransit.Shops.Requests;
using Shops.Domain.Services;

namespace Shops.API.Consumers
{
    public class BuyProducts : ShopsBaseConsumer, IConsumer<BuyProductsRequest>
    {
        private readonly IBusControl _busControl;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/purchasesQueue");

        public BuyProducts(IShopsService shopsService,
            IBusControl busControl) : base(shopsService)
        {
            _busControl = busControl;
        }

        public async Task Consume(ConsumeContext<BuyProductsRequest> context)
        {
            var order = await ShopsService.BuyProducts(context.Message.ShopId, context.Message.Products);
            await context.RespondAsync(order);
            if (order.Exception is null)
            {
                var transaction =
                    await ShopsService.CreateTransaction(context.Message.ShopId, order.Content);
                await ShopsService.AddReceipt(transaction.Receipt);
                
                var endpoint = await _busControl.GetSendEndpoint(_rabbitMqUrl);
                await endpoint.Send(new AddTransactionRequest
                {
                    User = context.Message.User,
                    Transaction = transaction
                });
            }
        }
    }
}
