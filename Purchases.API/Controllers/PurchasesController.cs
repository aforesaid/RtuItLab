using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Purchases.API.Models.ViewModels;
using Purchases.API.Services;

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
        public async Task<ICollection<string>> GetHistory(string id)
        {
            return null;
        }
        /// <summary>
        /// Добавить транзакцию
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AddTransaction([FromBody] Transaction transaction)
        {
            if (ModelState.IsValid)
            {


            }
            return null;
        }
        /// <summary>
        /// Обновить транзакцию, если можно
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> UpdateTransaction()
        {
            return null;
        }
    }
}
