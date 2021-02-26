using Microsoft.AspNetCore.Mvc;
using ServicesDtoModels.Models.Shops;
using Shops.API.Helpers;
using Shops.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IShopsService _shopsService;

        public ShopsController(IShopsService shopsService)
        {
            _shopsService = shopsService;
        }

        [HttpGet("shops")]
        public IActionResult GetAllShops()
        {
            return Ok(_shopsService.GetAllShops());
        }

        [HttpGet("shops/{id}")]
        public async Task<IActionResult> GetProducts(int shopId)
        {
            var products = await _shopsService.GetProductsByShop(shopId);
            return Ok(products);
        }

        [HttpPost("shops/{id]/find_by_category")]
        public async Task<IActionResult> GetProductsByCategory(int shopId, [FromBody] string categoryName)
        {
            if (!ModelState.IsValid) return BadRequest();
            var products = await _shopsService.GetProductsByCategory(shopId, categoryName);
            return Ok(products);
        }

        [Authorize]
        [HttpPost("shops/{id}/order")]
        public async Task<IActionResult> BuyProducts(int shopId, [FromBody] ICollection<Product> products)
        {
            if (!ModelState.IsValid) return BadRequest();
            var response = await _shopsService.BuyProducts(shopId, products);
            if (response == "Success")
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("updateProduct")]
        public async Task<IActionResult> AddProducts([FromBody] ICollection<ProductByFactory> products)
        {
            if (!ModelState.IsValid) return BadRequest();
                await _shopsService.AddProductsByFactory(products);
            return Ok();
        }

    }
}
