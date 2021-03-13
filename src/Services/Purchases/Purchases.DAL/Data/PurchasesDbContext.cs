using Microsoft.EntityFrameworkCore;
using Purchases.DAL.ContextModels;

namespace Purchases.DAL.Data
{
    public sealed class PurchasesDbContext : DbContext
    {
        public PurchasesDbContext(DbContextOptions<PurchasesDbContext> options) :
            base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<CustomerContext> Customers { get; set; }
    }
}
