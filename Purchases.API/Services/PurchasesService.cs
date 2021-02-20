using Purchases.API.Data;

namespace Purchases.API.Services
{
    public class PurchasesService : IPurchasesService
    {
        private readonly PurchasesDbContext _context; 
        public PurchasesService(PurchasesDbContext context)
        {
            _context = context;
        }
    }
}
