using System;
using System.Collections.Generic;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Purchases.API.Helpers;
using ServicesDtoModels.Models.Identity;
using ServicesDtoModels.Models.Purchases;
using System.Threading.Tasks;
using RtuItLab.Infrastructure.MassTransit.Requests.Purchases;
using RtuItLab.Infrastructure.MassTransit.Responds.Purchases;

namespace Purchases.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //TODO: добавить миддлвеер для авторизации с кроликом
    public class PurchasesController : ControllerBase
    {
        private readonly IBusControl _busControl;
        public PurchasesController(IBusControl busControl)
        {
            _busControl = busControl;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllHistory()
        {
            var user = HttpContext.Items["User"] as User;
            var serviceAddress = new Uri("rabbitmq://localhost/purchasesQueue");
            var client = _busControl.CreateRequestClient<User>(serviceAddress);
            var response = await client.GetResponse<GetTransactionsResponse>(user);
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHistory(int id)
        {
            var user = HttpContext.Items["User"] as User;
            var serviceAddress = new Uri("rabbitmq://localhost/purchasesQueue");
            var request = new GetTransactionByIdRequest
            {
                User = user,
                Id = id
            };
            var client = _busControl.CreateRequestClient<GetTransactionByIdRequest>(serviceAddress);
            var response = await client.GetResponse<Transaction>(request);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var user     = HttpContext.Items["User"] as User;
            var serviceAddress = new Uri("rabbitmq://localhost/purchasesQueue");
            var request = new AddTransactionRequest()
            {
                User = user,
                Transaction = transaction
            };
            var client = _busControl.CreateRequestClient<AddTransactionRequest>(serviceAddress);
            var response = await client.GetResponse<object>(request);
            return Ok(response);
        }
        /// <summary>
        /// Обновить транзакцию, если можно
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateTransaction( [FromBody] UpdateTransaction updateTransaction)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request");
            var user = HttpContext.Items["User"] as User;
            var serviceAddress = new Uri("rabbitmq://localhost/purchasesQueue");
            var request = new UpdateTransactionRequest()
            {
                User = user,
                Transaction = updateTransaction
            };
            var client = _busControl.CreateRequestClient<UpdateTransactionRequest>(serviceAddress);
            var response = await client.GetResponse<UpdateTransactionRespond>(request);
            return Ok(response.Message);
        }
    }
}
