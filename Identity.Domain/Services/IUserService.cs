using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RtuItLab.Infrastructure.Models.Identity;

namespace Identity.Domain.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(LoginRequest model);
        Task<User> GetUserById(string id);
        Task<IdentityResult> CreateUser(AuthenticateRequest model);
        public Task<User> GetUserByToken(TokenRequest model);
    }
}
