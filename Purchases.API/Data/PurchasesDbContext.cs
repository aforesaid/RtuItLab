using Microsoft.EntityFrameworkCore;
using Purchases.API.Models.ContextModels;

namespace Purchases.API.Data
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
