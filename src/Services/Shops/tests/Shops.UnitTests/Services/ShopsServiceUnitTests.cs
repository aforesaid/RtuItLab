using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RtuItLab.Infrastructure.Exceptions;
using RtuItLab.Infrastructure.Models.Shops;
using Shops.DAL.Data;
using Shops.Domain.Services;
using Xunit;

namespace Shops.UnitTests.Services
{
    public class ShopsServiceUnitTests
    {
        private readonly ShopsDbContext _shopsContext;
        private readonly ShopsService _shopsService;

        public ShopsServiceUnitTests()
        {
            var options = new DbContextOptionsBuilder<ShopsDbContext>()
                .UseInMemoryDatabase("test_shops");
            _shopsContext = new ShopsDbContext(options.Options);
            _shopsService = new ShopsService(_shopsContext);
        }

        [Fact] 
        public void GetAllShop_Expected_Shops_And_Null_products()
        {
            var response = _shopsService.GetAllShops();
            var shops = response.Content;
            Assert.NotNull(shops);
            Assert.Null(shops.First().Products);
        }
        [Fact]
        public async Task GetProductsByShopId_Expected_Shops_And_Included_All_products()
        {
            var response = await _shopsService.GetProductsByShop(1);
            var product = response.Content;
            Assert.NotNull(product);
            Assert.NotNull(product.First());
        }
        [Fact]
        public async Task GetProductsByShopId_Expected_Throw_NotFoundException()
        {
            var response = await _shopsService.GetProductsByShop(1251252151);
            Assert.NotNull(response.Exception);
            Assert.IsType<NotFoundException>(response.Exception);
        }
        [Fact]
        public async Task GetProductsByCategoryInShop_Expected_Throw_NotFoundException()
        {
            var response = await _shopsService.GetProductsByShop(1251252151);
            Assert.NotNull(response.Exception);
            Assert.IsType<NotFoundException>(response.Exception);
        }
        [Fact]
        public async Task GetProductsByCategoryInShop_Expected_Success()
        {
            var response = await _shopsService.GetProductsByCategory(1,"одежда");
            var product = response.Content;
            Assert.NotNull(product);
            Assert.NotNull(product.First());
        }
        [Fact]
        public async Task BuyProductsInShop_Expected_Throw_BadRequestException()
        {
            var response = await _shopsService.BuyProducts(115215125, new List<Product>());
            Assert.NotNull(response.Exception);
            Assert.IsType<BadRequestException>(response.Exception);
        }
        [Fact]
        public async Task BuyProductsInShop_Expected_Throw_BadRequestException_Invalid_List_Product()
        {
            var response = await _shopsService.BuyProducts(1, new List<Product>());
            Assert.NotNull(response.Exception);
            Assert.IsType<BadRequestException>(response.Exception);
        }
        [Fact]
        public async Task BuyProductsInShop_Expected_Throw_BadRequestException_Invalid_Product()
        {
            var response = await _shopsService.BuyProducts(1, new List<Product>
            {
                new Product(){ProductId = 6,Count = 1}
            });
            Assert.NotNull(response.Exception);
            Assert.IsType<BadRequestException>(response.Exception);
        }
    }
}