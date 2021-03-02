using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Purchases.API.Helpers;
using RtuItLab.Infrastructure.MassTransit.Purchases.Requests;
using RtuItLab.Infrastructure.MassTransit.Purchases.Responses;
using RtuItLab.Infrastructure.Models.Identity;
using RtuItLab.Infrastructure.Models.Purchases;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Purchases.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchasesController : ControllerBase
    {
        private readonly IBusControl _busControl;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/purchasesQueue");

        public PurchasesController(IBusControl busControl)
        {
            _busControl = busControl;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllHistory()
        {
            var user = HttpContext.Items["User"] as User;
            var client = _busControl.CreateRequestClient<User>(_rabbitMqUrl);
            var response = await client.GetResponse<GetTransactionsResponse>(user);
            return Ok(response.Message);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHistory(int id)
        {
            var user = HttpContext.Items["User"] as User;
            var client = _busControl.CreateRequestClient<GetTransactionByIdRequest>(_rabbitMqUrl);
            var response = await client.GetResponse<Transaction>(new GetTransactionByIdRequest
            {
                User = user,
                Id = id
            });
            return Ok(response.Message);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var user     = HttpContext.Items["User"] as User;
            if (transaction.IsShopCreate)
                return BadRequest(new ValidationException("You can't add shops' transaction"));
            if (transaction.Receipt != null)
                return BadRequest(new ValidationException("Receipt must be null! Use \"receipt\":null in your request"));
            var client = _busControl.CreateRequestClient<AddTransactionRequest>(_rabbitMqUrl);
            var response = await client.GetResponse<AddTransactionResponse>(new AddTransactionRequest()
            {
                User = user,
                Transaction = transaction
            });
            return Ok(response.Message);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTransaction( [FromBody] UpdateTransaction updateTransaction)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var user = HttpContext.Items["User"] as User;
            var client = _busControl.CreateRequestClient<UpdateTransactionRequest>(_rabbitMqUrl);
            var response = await client.GetResponse<UpdateTransactionResponse>(new UpdateTransactionRequest()
            {
                User = user,
                Transaction = updateTransaction
            });
            return Ok(response.Message);
        }
    }
}
