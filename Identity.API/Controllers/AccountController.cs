using Identity.API.Helpers;
using Identity.DAL.ContextModels;
using Identity.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesDtoModels.Models.Identity;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        public AccountController(UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequest model)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = model.Username
            };
            var user = await _userManager.FindByNameAsync(model.Username);
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var request = new AuthenticateRequest
                {
                    Username = model.Username,
                    Password = model.Password
                };
                var response = await _userService.Authenticate(request);
                return Ok(response);
            }
            return BadRequest(new { message = "Username or password is incorrect" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthenticateRequest model)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = model.Username
            };
            var response = await _userManager.CreateAsync(applicationUser, model.Password);
            return Ok(response);
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUser()
        {
            var user = HttpContext.Items["User"] as User;
            return Ok(user);
        }



    }
}
