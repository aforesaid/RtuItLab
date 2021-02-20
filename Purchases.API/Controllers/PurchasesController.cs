using Microsoft.AspNetCore.Mvc;
using Purchases.API.Models.ViewModels;
using Purchases.API.Services;
using System.Threading.Tasks;

namespace Purchases.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchasesService _purchasesService;
        public PurchasesController(IPurchasesService purchasesService)
        {
            _purchasesService = purchasesService;
        }
        [HttpGet("{userid}/{id}")]
        public async Task<IActionResult> GetHistory(string id,[FromBody] string userId)
        {
            //TODO: проверить работает ли, когда Id пустой
            if (id != null && !int.TryParse(id, out _))
                return BadRequest($"Invalid id: {id}");
            return id is null ? Ok(await _purchasesService.GetTransactions(userId)) 
                : Ok(await _purchasesService.GetTransactionById(userId, int.Parse(id)));
        }
        [HttpPost("{userid}")]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction, string userId)
        {
            if (ModelState.IsValid)
            {
                var response = await _purchasesService.AddTransaction(userId, transaction);
                return Ok(response);
            }
            return BadRequest("Invalid request");
        }
        /// <summary>
        /// Обновить транзакцию, если можно
        /// </summary>
        /// <returns></returns>
        [HttpPut("{userid}")]
        public async Task<IActionResult> UpdateTransaction(string userId, [FromBody] UpdateTransaction updateTransaction)
        {
            if (ModelState.IsValid)
            {
                var (content, successed) = await _purchasesService.UpdateTransaction(userId, updateTransaction);
                if (successed)
                    return Ok(content);
                return Forbid(content);
            }
            return BadRequest("Invalid request");
        }
    }
}
