using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.API.Models;

namespace Identity.API.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<ApplicationUser> GetUserById(string id);
    }
}
