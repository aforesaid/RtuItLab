using Identity.API.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RtuItLab.Infrastructure.Models.Identity;
using ServicesDtoModels.Models.Identity;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IBusControl _busControl;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/identityQueue");

        public AccountController(IBusControl busControl)
        {
            _busControl = busControl;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequest model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var client = _busControl.CreateRequestClient<LoginRequest>(_rabbitMqUrl);
            var response = await client.GetResponse<AuthenticateResponse>(model);
            if (response.Message.Success)
                return Ok(response.Message);
            return BadRequest(new {message = "Username or password is incorrect"});
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthenticateRequest model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var client = _busControl.CreateRequestClient<AuthenticateRequest>(_rabbitMqUrl);
            var response = await client.GetResponse<IdentityResult>(model);
            return Ok(response.Message);
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
