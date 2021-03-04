using RtuItLab.Infrastructure.Models.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RtuItLab.Infrastructure.MassTransit;

namespace Identity.Domain.Services
{
    public interface IUserService
    {
        Task<ResponseMassTransit<AuthenticateResponse>> Authenticate(AuthenticateRequest model);
        Task<ResponseMassTransit<User>> GetUserById(string id);
        Task<ResponseMassTransit<IdentityResult>> CreateUser(RegisterRequest model); 
        Task<ResponseMassTransit<User>> GetUserByToken(TokenRequest model);
    }
}
