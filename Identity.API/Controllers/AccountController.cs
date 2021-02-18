using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Identity.API.Data;


namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost("login")]
        public async Task<string> Login([FromBody] AuthorizeModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = model.Login
            };
            var user = await _userManager.FindByNameAsync(model.Login);
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {

            }

            return null;

        }

        [HttpPost("register")]
        public async Task<string> Register([FromBody] AuthorizeModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = model.Login
            };
            var s = await _userManager.CreateAsync(applicationUser, model.Password);
            return null;
        }

    }
}
