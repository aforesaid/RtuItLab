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
        public DbSet<ReceiptContext> Receipts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopContext>().HasData(new ShopContext
            {
                Id = 1,
                PhoneNumber = "79788994545",
                Address = "Москва, ул. Первозданного 36",
            }, new ShopContext
            {
                Id = 2,
                PhoneNumber = "79788992113",
                Address = "Орёл, ул. Маршалла 36"
            }, new ShopContext
            {
                Id = 3,
                PhoneNumber = "79788992553",
                Address = "Москва, ул. Первомайская 36"
            });

            modelBuilder.Entity<ProductContext>().HasData(new ProductContext
            {
                ShopContextKey = 1,
                Id = 1,
                Category = "одежда",
                Count = 100,
                Name = "Трусы",
                Cost = 123.12m
            }, new ProductContext
            {
                ShopContextKey = 1,
                Id = 2,
                Category = "одежда",
                Count = 100,
                Name = "Штаны",
                Cost = 1
            }, new ProductContext
            {
                ShopContextKey = 1,
                Id = 3,
                Category = "обувь",
                Count = 100,
                Name = "Бутекс",
                Cost = 2123
            }, new ProductContext
            {
                ShopContextKey = 1,
                Id = 4,
                Category = "обувь",
                Count = 100,
                Name = "Шлепанцы",
                Cost = 122
            }, new ProductContext 
            {
                ShopContextKey = 2,
                Id = 5,
                Category = "обувь",
                Count = 100,
                Name = "Ботильоны",
                Cost = 50

            }, new ProductContext 
            {
                ShopContextKey = 2,
                Id = 6,
                Category = "еда",
                Count = 100,
                Name = "Чебурек",
                Cost = 50
            }, new ProductContext 
            {
                ShopContextKey = 2,
                Id = 7,
                Category = "еда",
                Count = 1002,
                Name = "Самса",
                Cost = 50
            }, new ProductContext 
            {
                ShopContextKey = 3,
                Id = 8,
                Category = "строительные материалы",
                Count = 100,
                Name = "Кирпич",
                Cost = 50
            }, new ProductContext
            {
                ShopContextKey = 3,
                Id = 9,
                Category = "строительные материалы",
                Count = 23,
                Name = "Асфальт",
                Cost = 50
            }
            );
        }
    }
}
