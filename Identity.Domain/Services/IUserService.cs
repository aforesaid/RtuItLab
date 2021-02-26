using System.Threading.Tasks;
using ServicesDtoModels.Models.Identity;

namespace Identity.Domain.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<User> GetUserById(string id);
    }
}
