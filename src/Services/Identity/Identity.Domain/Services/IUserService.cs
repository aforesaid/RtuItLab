using RtuItLab.Infrastructure.Models.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RtuItLab.Infrastructure.MassTransit;

namespace Identity.Domain.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<User> GetUserById(string id);
        Task<IdentityResult> CreateUser(RegisterRequest model); 
        Task<User> GetUserByToken(TokenRequest model);
    }
}
