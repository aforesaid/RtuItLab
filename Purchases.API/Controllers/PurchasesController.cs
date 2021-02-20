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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHistory(string id)
        {
            //TODO: проверить работает ли, когда Id пустой
            if (id != null && !int.TryParse(id, out _))
                return BadRequest($"Invalid id: {id}");
            return id is null ? Ok(await _purchasesService.GetTransactions()) 
                : Ok(await _purchasesService.GetTransactionById(int.Parse(id)));
        }
        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var response = await _purchasesService.AddTransaction(transaction);
                return Ok(response);
            }
            return BadRequest("Invalid request");
        }
        /// <summary>
        /// Обновить транзакцию, если можно
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateTransaction([FromBody] UpdateTransaction updateTransaction)
        {
            if (ModelState.IsValid)
            {
                var (content, successed) = await _purchasesService.UpdateTransaction(updateTransaction);
                if (successed)
                    return Ok(content);
                return Forbid(content);
            }
            return BadRequest("Invalid request");
        }
    }
}
