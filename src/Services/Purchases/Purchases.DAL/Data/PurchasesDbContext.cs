using Microsoft.EntityFrameworkCore;
using Purchases.DAL.ContextModels;

namespace Purchases.DAL.Data
{
    public class PurchasesDbContext : DbContext
    {
        public PurchasesDbContext(DbContextOptions<PurchasesDbContext> options) :
            base(options)
        {
            
        }
        public DbSet<CustomerContext> Customers { get; set; }
    }
}
