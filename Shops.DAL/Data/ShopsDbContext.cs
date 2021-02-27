using Microsoft.EntityFrameworkCore;
using Shops.DAL.ContextModels;

namespace Shops.DAL.Data
{
    public class ShopsDbContext : DbContext
    {
        public ShopsDbContext(DbContextOptions<ShopsDbContext> options)
        : base(options)
        {
            
        }
        public DbSet<ShopContext> Shops { get; set; }
    }
}
