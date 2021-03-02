using RtuItLab.Infrastructure.Models.Shops;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Factories.Domain.Services
{
    public interface IFactoriesService
    {
        public Task<ICollection<ProductByFactory>> CreateRequestInShops();
    }
}
