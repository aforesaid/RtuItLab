using Identity.FunctionalTests.Base;
using Microsoft.AspNetCore.TestHost;
using RtuItLab.Infrastructure.Models.Identity;
using RtuItLab.Infrastructure.Models.Shops;
using Shops.FunctionalTests.Base;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RtuItLab.Infrastructure.Models;
using Xunit;

namespace Shops.FunctionalTests
{
    public class ShopsScenarios : ShopsScenariosBase
    {
        private const string RequestType = "application/json";
        private string _token;
        private TestServer _testServerIdentity;
        public ShopsScenarios()
        {
            Configure().Wait();
        }
        private async Task Configure()
        {
            _testServerIdentity = new Identity.FunctionalTests.IdentityScenarios().CreateServer();
            _token = await GetToken();
        }
        [Fact]
        public async Task Get_all_shops_response_ok_status_code()
        {
            using var server = CreateServer();
            var response = await server.CreateClient().GetAsync(Get.AllShops);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_products_in_shop_response_ok_status_code()
        {
            using var server = CreateServer();
            var response = await server.CreateClient().GetAsync(Get.ProductsInShop);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Post_find_products_in_first_shop_by_category_response_ok_status_code()
        {
            using var server = CreateServer();
            var content = new StringContent(BuildCategoryRequest(), Encoding.UTF8, RequestType);
            var response = await server.CreateClient().PostAsync(Post.FindProductsInFirstShopByCategory,content);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Post_buy_products_response_ok_status_code()
        {
            using var server = CreateServer();
            using var client = server.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            var content = new StringContent(BuildListProducts(), Encoding.UTF8, RequestType);
            var response = await client.PostAsync(Post.BuyProductsInFirstShop,content);
            response.EnsureSuccessStatusCode();
        }
        private async Task<string> GetToken()
        {
            using var client = _testServerIdentity.CreateClient();
            var content = new StringContent(BuildUser(), Encoding.UTF8, RequestType);
            await client.PostAsync(IdentityScenariosBase.Post.Register, content);
            var response = await client.PostAsync(IdentityScenariosBase.Post.Login, content);
            var contentString = await response.Content.ReadAsStringAsync();
            var userInfo = DeserializeResponse<ApiResult<AuthenticateResponse>>(contentString).Result;
            return userInfo.Token;
        }
        private static string BuildUser()
        {
            var user = new RegisterRequest
            {
                Username = "admin",
                Password = "Pass1234"
            };
            return JsonSerializer.Serialize(user);
        }
        private static string BuildCategoryRequest()
        {
            var response = new Category
            {
                CategoryName = "Очки"
            };
            return JsonSerializer.Serialize(response);
        }

        private static string BuildListProducts()
        {
            var response = new List<Product>
            {
                new Product
                {
                    Category = "шаурмички",
                    Count = 1,
                    Cost = 3,
                    Name = "шавуха",
                    ProductId = 1
                },
                new Product
                {
                    ProductId = 2,
                    Category = "шаурмички",
                    Count = 100,
                    Name = "пирожки",
                    Cost = 1
                }
            };
            return JsonSerializer.Serialize(response);
        }
        private static T DeserializeResponse<T>(string content)
            => JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        ~ShopsScenarios()
        {
            _testServerIdentity.Dispose();
        }
    }
}
