using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RtuItLab.Infrastructure.Filters;
using RtuItLab.Infrastructure.MassTransit;
using RtuItLab.Infrastructure.Models;
using RtuItLab.Infrastructure.Models.Identity;
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
            var response = await GetResponseRabbitTask<AuthenticateRequest, AuthenticateResponse>(model);
            return Ok(ApiResult<AuthenticateResponse>.Success200(response));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var response = await GetResponseRabbitTask<RegisterRequest, IdentityResult>(model);
            return Ok(ApiResult<IdentityResult>.Success200(response));
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUser()
        {
            var user = HttpContext.Items["User"] as User;
            return Ok(ApiResult<User>.Success200(user));
        }

        private async Task<TOut> GetResponseRabbitTask<TIn,TOut>(TIn request)
        where TIn: class
        where TOut: class
        {
            var client = _busControl.CreateRequestClient<TIn>(_rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }
    }
}
