using Microsoft.AspNetCore.Mvc;
using Purchases.API.Models.ViewModels;
using Purchases.API.Services;
using System.Threading.Tasks;
using Purchases.API.Helpers;
using Purchases.API.Models.DTOs;
using WebRabbitMQ;

namespace Purchases.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //TODO: добавить миддлвеер для авторизации с кроликом
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchasesService _purchasesService;
        private readonly IEventBus _eventBus;
        public PurchasesController(IPurchasesService purchasesService,
            IEventBus eventBus)
        {
            _purchasesService = purchasesService;
            _eventBus         = eventBus;
        }
        [HttpGet("{userid}/{id}")]
        public async Task<IActionResult> GetHistory(string id)
        {
            _eventBus.Publish("History");
            var user = HttpContext.Items["User"] as UserDTO;
            //TODO: проверить работает ли, когда Id пустой
            if (id != null && !int.TryParse(id, out _))
                return BadRequest($"Invalid id: {id}");
            return id is null ? Ok(await _purchasesService.GetTransactions(user)) 
                : Ok(await _purchasesService.GetTransactionById(user, int.Parse(id)));
        }
        [HttpPost("{userid}")]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            _eventBus.Publish("New Transaction");
            if (ModelState.IsValid)
            {
                var user     = HttpContext.Items["User"] as UserDTO;
                await _purchasesService.AddTransaction(user, transaction);
                return Ok();
            }
            return BadRequest("Invalid request");
        }
        /// <summary>
        /// Обновить транзакцию, если можно
        /// </summary>
        /// <returns></returns>
        [HttpPut("{userid}")]
        public async Task<IActionResult> UpdateTransaction( [FromBody] UpdateTransaction updateTransaction)
        {
            _eventBus.Publish("Update Transaction");
            if (ModelState.IsValid)
            {
                var user = HttpContext.Items["User"] as UserDTO;
                var (content, isSuccess) = await _purchasesService.UpdateTransaction(user, updateTransaction);
                if (isSuccess)
                    return Ok();
                return Forbid(content);
            }
            return BadRequest("Invalid request");
        }
    }
}
