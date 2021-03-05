using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Shops.FunctionalTests.Base
{
    public class ShopsScenariosBase
    {
        private const string ApiUrlBase = "api/shops";

        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(ShopsScenariosBase))?.Location;
            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json", optional: false)
                        .AddEnvironmentVariables();
                })
                .UseUrls("http://*:7003")
                .UseStartup<API.Startup>();
            return new TestServer(hostBuilder);
        }
        public static class Get
        {
            public static string AllShops = ApiUrlBase;
            public static string ProductsInShop = $"{ApiUrlBase}/1";
        }
        public static class Post
        {
            public static string FindProductsInFirstShopByCategory = $"{ApiUrlBase}/1/find_by_category";
            public static string BuyProductsInFirstShop = $"{ApiUrlBase}/1/order";
        }

    }
}
