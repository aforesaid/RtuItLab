using Microsoft.AspNetCore.Mvc;
using Shops.API.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shops.API.Services
{
    public interface IShopsService
    {
        public ICollection<Shop> GetAllShops();
        public Task<ICollection<Product>> GetProductsByShop(int shopId);
        public Task<ICollection<Product>> GetProductsByCategory(int shopId, string categoryName);
        public Task<string> BuyProducts([FromBody] int shopId, [FromBody] ICollection<Product> products);
    }
}
