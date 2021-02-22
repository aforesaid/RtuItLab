using Factories.API.Data;
using System.Threading.Tasks;

namespace Factories.API.Services
{
    public class FactoriesService : IFactoriesService
    {
        private FactoriesDbContext _context;
        public FactoriesService(FactoriesDbContext context)
        {
            _context = context;
        }

        public Task<string> CreateRequestInShops()
        {
            //TODO: дописать запрос
            return null;
        }
    }
}
