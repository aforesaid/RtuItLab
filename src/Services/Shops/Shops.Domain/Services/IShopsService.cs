using System.Collections.Generic;
using System.Threading.Tasks;
using RtuItLab.Infrastructure.MassTransit;
using RtuItLab.Infrastructure.Models.Purchases;
using RtuItLab.Infrastructure.Models.Shops;

namespace Shops.Domain.Services
{
    public interface IShopsService
    {
        public ResponseMassTransit<ICollection<Shop>> GetAllShops();
        public Task<ResponseMassTransit<ICollection<Product>>> GetProductsByShop(int shopId);
        public Task<ResponseMassTransit<ICollection<Product>>> GetProductsByCategory(int shopId, string categoryName);
        public Task<ResponseMassTransit<BaseResponseMassTransit>> BuyProducts(int shopId, ICollection<Product> products);
        public Task AddProductsByFactory(ICollection<ProductByFactory> products);
        public Task<Transaction> CreateTransaction(int shopId, ICollection<Product> products);
    }
}
