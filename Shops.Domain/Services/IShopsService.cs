using System.Collections.Generic;
using System.Threading.Tasks;
using ServicesDtoModels.Models.Shops;

namespace Shops.Domain.Services
{
    public interface IShopsService
    {
        public ICollection<Shop> GetAllShops();
        public Task<ICollection<Product>> GetProductsByShop(int shopId);
        public Task<ICollection<Product>> GetProductsByCategory(int shopId, string categoryName);
        public Task<(string,bool)> BuyProducts(int shopId, ICollection<Product> products);
        public Task AddProductsByFactory(ICollection<ProductByFactory> products);
    }
}
