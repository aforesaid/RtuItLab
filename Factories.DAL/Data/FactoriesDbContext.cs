using Factories.DAL.ContextModels;
using Microsoft.EntityFrameworkCore;

namespace Factories.DAL.Data
{
    public sealed class FactoriesDbContext : DbContext
    {
        public FactoriesDbContext( DbContextOptions<FactoriesDbContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<FactoryContext> Factories { get; set; }
        public DbSet<ProductByFactoryContext> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FactoryContext>().HasData(new FactoryContext[]
            {
                new FactoryContext
                {
                    Id = 1
                },
                new FactoryContext
                {
                    Id = 2
                },
                new FactoryContext
                {
                    Id = 3
                }
            });
            modelBuilder.Entity<ProductByFactoryContext>().HasData(new ProductByFactoryContext[]
            {
                new ProductByFactoryContext
                {
                    Count = 3,
                    FactoryContextKey = 1,
                    Id = 1,
                    PartsOfCreate = 2,
                    ProductId = 1,
                    ShopId = 1
                },
                new ProductByFactoryContext
                {
                    Count = 2,
                    FactoryContextKey = 1,
                    Id = 2,
                    PartsOfCreate = 3,
                    ProductId = 2,
                    ShopId = 1
                },
                new ProductByFactoryContext
                {
                    Count = 3,
                    FactoryContextKey = 1,
                    Id = 3,
                    PartsOfCreate = 1,
                    ProductId = 4,
                    ShopId = 2
                },
                new ProductByFactoryContext
                {
                    Count = 3,
                    FactoryContextKey = 1,
                    Id = 8,
                    PartsOfCreate = 2,
                    ProductId = 9,
                    ShopId = 3
                },
                new ProductByFactoryContext
                {
                    Count = 3,
                    FactoryContextKey = 2,
                    Id = 4,
                    PartsOfCreate = 2,
                    ProductId = 5,
                    ShopId = 2
                },
                new ProductByFactoryContext
                {
                    Count = 3,
                    FactoryContextKey = 3,
                    Id = 5,
                    PartsOfCreate = 2,
                    ProductId = 6,
                    ShopId = 2
                },
                new ProductByFactoryContext
                {
                    Count = 3,
                    FactoryContextKey = 2,
                    Id = 6,
                    PartsOfCreate = 2,
                    ProductId = 1,
                    ShopId = 1
                },
                new ProductByFactoryContext
                {
                    Count = 3,
                    FactoryContextKey = 2,
                    Id = 7,
                    PartsOfCreate = 2,
                    ProductId = 1,
                    ShopId = 1
                }
            });
        }
    }
}
