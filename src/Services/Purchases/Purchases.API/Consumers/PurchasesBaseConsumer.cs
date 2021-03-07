using Purchases.Domain.Services;

namespace Purchases.API.Consumers
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
