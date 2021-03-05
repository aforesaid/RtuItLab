using Identity.FunctionalTests.Base;
using RtuItLab.Infrastructure.Models.Identity;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using RtuItLab.Infrastructure.Models;
using Xunit;

namespace Identity.FunctionalTests
{
    public class IdentityScenarios : IdentityScenariosBase
    {
        private const string RequestType = "application/json";

        [Fact]
        public async Task Post_register_user_response_ok_status_code()
        {
            using var server = CreateServer();
            var content = new StringContent(BuildUser(), Encoding.UTF8, RequestType);
            var response = await server.CreateClient().PostAsync(Post.Register, content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_login_user_response_ok_status_code()
        {
            using var server = CreateServer();
            var content = new StringContent(BuildUser(), Encoding.UTF8, RequestType);
            var response = await server.CreateClient().PostAsync(Post.Login, content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_user_info_response_ok_status_code()
        {
            using var server = CreateServer();
            using var client = server.CreateClient();
            var content = new StringContent(BuildUser(), Encoding.UTF8, RequestType);
            var response = await client.PostAsync(Post.Login, content);
            response.EnsureSuccessStatusCode();
            var contentString = await response.Content.ReadAsStringAsync();
            var userInfo = DeserializeUserResponse(contentString);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userInfo.Result.Token}");
            response = await client.GetAsync(Get.GetInfoByUser);
            response.EnsureSuccessStatusCode();
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

        public static ApiResult<AuthenticateResponse> DeserializeUserResponse(string content)
            => JsonSerializer.Deserialize<ApiResult<AuthenticateResponse>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
    }
}
