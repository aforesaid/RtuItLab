﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<string> BuyProducts(int shopId, ICollection<Product> products)
        {
            if (products.Count > MaxProductRequestCount)
                return $"Too many products, max count is {MaxProductRequestCount}";
            var shop = await _context.Shops.Include(item => item.Products)
                .FirstOrDefaultAsync(item => item.Id == shopId);
            var isSuccess = products.Select(product =>
            {
                var item = shop.Products.FirstOrDefault(productContext => productContext.Id == product.ProductId);
                if (item is null || item.Count < product.Count)
                    return "BadRequest";
                item.Count -= product.Count;
                return "Success";
            }).Any(item => item != "BadRequest");
            if (!isSuccess) return "No product found";
            await _context.SaveChangesAsync();
            return "Success";
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
                var productContext = shop.Products.FirstOrDefault(item => item.ProductId == product.ProductId);
                if (productContext != null)
                    productContext.Count += product.Count;
            });
        }
    }
}