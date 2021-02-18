using Factories.API.Models.ContextModels;
using Microsoft.EntityFrameworkCore;

namespace Factories.API.Data
{
    public class FactoriesDbContext : DbContext
    {
        public FactoriesDbContext( DbContextOptions<FactoriesDbContext> options)
        : base(options)
        {
            
        }
        public DbSet<Factory> Factories { get; set; }
    }
}
