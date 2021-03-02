using Factories.DAL.ContextModels;
using Microsoft.EntityFrameworkCore;

namespace Factories.DAL.Data
{
    public class FactoriesDbContext : DbContext
    {
        public FactoriesDbContext( DbContextOptions<FactoriesDbContext> options)
        : base(options)
        {
            
        }
        public DbSet<FactoryContext> Factories { get; set; }
    }
}
