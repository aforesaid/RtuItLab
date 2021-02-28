using Identity.API.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesDtoModels.Models.Identity;
using System;
using System.Threading.Tasks;
using RtuItLab.Infrastructure.Models.Identity;
using ServicesDtoModels.Models.Purchases;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IBusControl _busControl;

        public AccountController(IBusControl busControl)
        {
            _busControl = busControl;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequest model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var serviceAddress = new Uri("rabbitmq://localhost/identityQueue");
            var client = _busControl.CreateRequestClient<LoginRequest>(serviceAddress);
            var response = await client.GetResponse<AuthenticateResponse>(model);
            if (response.Message.Success)
                return Ok(response.Message);
            return BadRequest(new {message = "Username or password is incorrect"});
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthenticateRequest model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var serviceAddress = new Uri("rabbitmq://localhost/identityQueue");
            var client = _busControl.CreateRequestClient<AuthenticateRequest>(serviceAddress);
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
