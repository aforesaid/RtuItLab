using Identity.API.Models;
using Identity.API.Models.DTOs;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<UserDTO> GetUserById(string id);
    }
}
