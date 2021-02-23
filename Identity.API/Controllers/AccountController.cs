using Identity.API.Helpers;
using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Identity.API.Models.DTOs;
using WebRabbitMQ;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;
        public AccountController(UserManager<ApplicationUser> userManager,
            IUserService userService,
            IEventBus eventBus)
        {
            _userManager = userManager;
            _userService = userService;
            _eventBus    = eventBus;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthorizeModel model)
        {
            _eventBus.Publish("Login");
            var applicationUser = new ApplicationUser()
            {
                UserName = model.Login
            };
            var user = await _userManager.FindByNameAsync(model.Login);
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var request = new AuthenticateRequest
                {
                    Username = model.Login,
                    Password = model.Password
                };
                var response = await _userService.Authenticate(request);
                return Ok(response);
            }
            return BadRequest(new { message = "Username or password is incorrect" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthorizeModel model)
        {
            _eventBus.Publish("Register");

            var applicationUser = new ApplicationUser()
            {
                UserName = model.Login
            };
            var response = await _userManager.CreateAsync(applicationUser, model.Password);
            return Ok(response);
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUser()
        {
            _eventBus.Publish("UserInfo");
            var user = HttpContext.Items["User"] as UserDTO;
            return Ok(user);
        }



    }
}
