using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shops.DAL.ContextModels;

namespace Shops.DAL.Data
{
    public sealed class ShopsDbContext : DbContext
    {
        public ShopsDbContext(DbContextOptions<ShopsDbContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<ShopContext> Shops { get; set; }
        public DbSet<ProductContext> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopContext>().HasData(new ShopContext
                {
                    Id = 1,
                    PhoneNumber = "+77777777777",
                    Address = "Москва, ул. Первозданного 36",
                }, new ShopContext
                {
                    Id = 2,
                    PhoneNumber = "+99999999999",
                    Address = "Орёл, ул. Первозданного 36",
                });

            modelBuilder.Entity<ProductContext>().HasData(new ProductContext
            {
                ShopContextKey = 1,
                Id = 1,
                Category = "шаурмички",
                Count = 100,
                Name = "шавуха",
                Cost = 2
            }, new ProductContext
            {
                ShopContextKey = 1,
                Id = 2,
                Category = "шаурмички",
                Count = 100,
                Name = "пирожки",
                Cost = 1
            }, new ProductContext
            {
                Id = 3,
                Category = "шаурмичка",
                ShopContextKey = 2,
                Count = 100,
                Name = "шавухи",
                Cost = 2
            }, new ProductContext
            {
                ShopContextKey = 2,
                Id = 4,
                Category = "шаурмички",
                Count = 100,
                Name = "пирожки",
                Cost = 1
            });
        }

    }
}
