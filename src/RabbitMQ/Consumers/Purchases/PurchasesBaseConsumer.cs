using Purchases.Domain.Services;

namespace RabbitMQ.Consumers.Purchases
{
    public class PurchasesBaseConsumer
    {
        protected readonly IPurchasesService PurchasesService;
        public PurchasesBaseConsumer(IPurchasesService purchasesService)
        {
            PurchasesService = purchasesService;
        }
    }
}
