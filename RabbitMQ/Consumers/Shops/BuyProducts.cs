using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using RtuItLab.Infrastructure.MassTransit.Requests.Shops;
using Shops.Domain.Services;

namespace RabbitMQ.Consumers.Shops
{
    public class BuyProducts : ShopsBaseConsumer, IConsumer<BuyProductsRequest>
    {
        private readonly ILogger<BuyProducts> _logger;
        public BuyProducts(IShopsService shopsService,
            ILogger<BuyProducts> logger) : base(shopsService)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BuyProductsRequest> context)
        {
            var (message,success) = await ShopsService.BuyProducts(context.Message.ShopId, context.Message.Products);
            _logger.LogInformation($"{message} {success}");
            await context.RespondAsync(new BuyProductsResponse{
            Success = success,
            Message = message
            });
        }
    }
}
