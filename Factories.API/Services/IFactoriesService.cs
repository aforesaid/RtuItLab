using System.Threading.Tasks;

namespace Factories.API.Services
{
    public interface IFactoriesService
    {
        public Task<string> CreateRequestInShops();
    }
}
