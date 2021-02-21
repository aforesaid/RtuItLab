using Microsoft.EntityFrameworkCore;
using Shops.API.Models.ContextModels;

namespace Shops.API.Data
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
