using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Purchases.FunctionalTests.Base
{
    public class PurchasesScenariosBase
    {
        private const string ApiUrlBase = "api/purchases";

        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(PurchasesScenariosBase))?.Location;
            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json", optional: false)
                        .AddEnvironmentVariables();
                })
                .UseUrls("http://*:7002")
                .UseStartup<API.Startup>();
            return new TestServer(hostBuilder);
        }
        public static class Get
        {
            public static string AllHistory = ApiUrlBase;
            public static string TransactionById = $"{ApiUrlBase}/1";
        }
        public static class Post
        {
            public static string AddTransaction = $"{ApiUrlBase}/add";
        }
        public static class Put
        {
            public static string UpdateTransaction = $"{ApiUrlBase}/update";
        }
    }
}
