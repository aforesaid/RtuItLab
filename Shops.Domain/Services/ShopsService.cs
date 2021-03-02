using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RtuItLab.Infrastructure.Models.Purchases;
using ServicesDtoModels.Models.Identity;
using ServicesDtoModels.Models.Purchases;
using ServicesDtoModels.Models.Shops;
using Shops.DAL.Data;
using Shops.Domain.Helpers;

namespace Shops.Domain.Services
{
    public class ShopsService : IShopsService
    {
        private const int MaxProductRequestCount = 10;
        private readonly ShopsDbContext _context;
        public ShopsService(ShopsDbContext context)
        {
            _context = context;
        }

        public ICollection<Shop> GetAllShops()
        => _context.Shops.Select(item => item.ToShopDto())
            .AsNoTracking()
            .ToList();

        public async Task<ICollection<Product>> GetProductsByShop(int shopId)
        {
            var shop = await _context.Shops.Include(item => item.Products)
                .FirstOrDefaultAsync(item => item.Id == shopId);
            return shop?.Products.Select(item => item.ToProductDto())
                .ToList();
        }

        public async Task<ICollection<Product>> GetProductsByCategory(int shopId, string categoryName)
        {
            var shop = await _context.Shops.Include(item => item.Products)
                .FirstOrDefaultAsync(item => item.Id == shopId);
            return shop?.Products.Where(item => item.Category == categoryName)
                .Select(item => item.ToProductDto())
                .ToList();
        }
        //TODO: пересмотреть валидацию (какая - то дичь)
        public async Task<(string,bool)> BuyProducts(int shopId, ICollection<Product> products)
        {
            if (products?.Count > MaxProductRequestCount)
                return ($"Too many products, max count is {MaxProductRequestCount}",false);
            var shop = await _context.Shops.Include(item => item.Products)
                .FirstOrDefaultAsync(item => item.Id == shopId);
            var isSuccess = ! products?.Any(product =>
            {
                var item = shop?.Products.FirstOrDefault(productContext => productContext.Id == product.ProductId);
                if (item is null || item.Count < product.Count)
                    return true;
                item.Count -= product.Count;
                return false;
            });
            switch (isSuccess)
            {
                case null:
                    return ("Invalid request", false);
                case false:
                    return ("No product found",false);
                default:
                    await _context.SaveChangesAsync();
                    return ("Success",true);
            }
        }
        public Task<Transaction> CreateTransaction(int shopId, ICollection<Product> products)
        {
            var response = new Transaction
            {
                Products = products.ToList(),
                Date = DateTime.Now,
                IsShopCreate = true,
                Receipt = new Receipt
                {
                    ShopId = shopId,
                    Cost = products.Sum(item => item.Cost* item.Count),
                    Count = products.Sum(item => item.Count),
                    Date = DateTime.Now
                }
            };
           return Task.FromResult(response);
        }
        public async Task AddProductsByFactory(ICollection<ProductByFactory> products)
        {
            var shopsCollection  = products.GroupBy(item => item.ShopId);
            shopsCollection.ToList().ForEach(async item =>
            {
                await AddProductsInShop(item.Key, item.ToList());
            });
            await _context.SaveChangesAsync();
        }
        private async Task AddProductsInShop(int shopId, List<ProductByFactory> products)
        {
            var shop = await _context.Shops.Include(item => item.Products)
                .FirstOrDefaultAsync(shopContext => shopContext.Id == shopId);
            products.ForEach(product =>
            {
                var productContext = shop.Products.FirstOrDefault(item => item.Id == product.ProductId);
                if (productContext != null)
                    productContext.Count += product.Count;
            });
        }
    }
}