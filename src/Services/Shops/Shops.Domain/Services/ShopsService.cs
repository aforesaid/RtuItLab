using Microsoft.EntityFrameworkCore;
using RtuItLab.Infrastructure.Exceptions;
using RtuItLab.Infrastructure.MassTransit;
using RtuItLab.Infrastructure.Models.Purchases;
using RtuItLab.Infrastructure.Models.Shops;
using Shops.DAL.Data;
using Shops.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shops.DAL.ContextModels;

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
        {
            var result = new List<Shop>();
            result.AddRange(
                    _context.Shops.Select(item => item.ToShopDto())
                    .AsNoTracking()
                    .ToList());
            return result;
        }

        public async Task<ICollection<Product>> GetProductsByShop(int shopId)
        {
            var result = new List<Product>();
            var shop = await _context.Shops.Include(item => item.Products)
                .FirstOrDefaultAsync(item => item.Id == shopId);
            
            if (shop is null)
                throw new NotFoundException("Shop not found");
            
            result = shop?.Products
                .Select(item => item.ToProductDto())
                .ToList();
            
            return result;
        }

        public async Task<ICollection<Product>> GetProductsByCategory(int shopId,
            string categoryName)
        {
            var shop = await _context.Shops.Include(item => item.Products)
                .FirstOrDefaultAsync(item => item.Id == shopId);
            if (shop is null)
                throw new NotFoundException("Shop not found");
            
            var result = shop?.Products.Where(item => item.Category == categoryName)
                .Select(item => item.ToProductDto())
                .ToList();
            
            return result;
        }

        public async Task<ICollection<Product>> BuyProducts(int shopId,
            ICollection<Product> products)
        {
            var resultProducts = new List<Product>();

            if (products.Count > MaxProductRequestCount)
                    throw new BadRequestException($"Too many products, max count is {MaxProductRequestCount}");
            
            if (products.Count < 1) 
                throw new BadRequestException($"Please, select products, max count is {MaxProductRequestCount}");
            
            var shop = await _context.Shops.Include(item => item.Products)
                .FirstOrDefaultAsync(item => item.Id == shopId);
            if (shop is null)
                throw new BadRequestException("Shop not found");
            foreach (var product in products)
            { 
                var item = shop?.Products.FirstOrDefault(productContext => productContext.Id == product.ProductId);
                if (item is null || item.Count < product.Count) 
                    throw new BadRequestException(
                        $"ProductId {product.ProductId} is either not found, or there not enough of it in the store");
                item.Count -= product.Count;
                var addProduct = item.ToProductDto();
                addProduct.Count = product.Count;
                resultProducts.Add(addProduct);
            }
                
            await _context.SaveChangesAsync();
                
            return resultProducts;
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
                    Cost = products.Sum(item => item.Cost * item.Count),
                    Count = products.Sum(item => item.Count),
                    Date = DateTime.Now,
                    Products = products.ToList()
                }
            };
            return Task.FromResult(response);
        }

        public async Task AddProductsByFactory(ICollection<ProductByFactory> products)
        {
            var shopsCollection = products.GroupBy(item => item.ShopId);
            shopsCollection.ToList().ForEach(async item => { await AddProductsInShop(item.Key, item.ToList()); });
            await _context.SaveChangesAsync();
        }

        public async Task AddReceipt(Receipt receipt)
        {
            var receiptContext = new ReceiptContext
            {
                FullCost = receipt.Products.Sum(item => item.Count* item.Cost),
                Count = receipt.Products.Sum(item => item.Count),
                Products = new List<ProductByReceiptContext>(receipt.Products.Select(item => item.ToProductByReceiptContext()))
            };
            await _context.Receipts.AddAsync(receiptContext);
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