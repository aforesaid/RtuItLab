using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Identity.FunctionalTests.Base
{
    public class IdentityScenariosBase
    {
        private const string ApiUrlBase = "api/account";

        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(IdentityScenariosBase))?.Location;
            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json", optional: false)
                        .AddEnvironmentVariables();
                })
                .UseUrls("http://*:7001")
                .UseStartup<API.Startup>();
            return new TestServer(hostBuilder);
        }

        public static class Get
        {
            public static string GetInfoByUser = $"{ApiUrlBase}/user";
        }
        public static class Post
        {
            public static string Login = $"{ApiUrlBase}/login";
            public static string Register = $"{ApiUrlBase}/register";
        }
    }
}
