using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shops.API.Services;
using System.Threading.Tasks;
using Shops.API.Helpers;
using Shops.API.Models.ViewModel;
using WebRabbitMQ;

namespace Shops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IShopsService _shopsService;
        private readonly IEventBus _eventBus;

        public ShopsController(IShopsService shopsService,
            IEventBus eventBus)
        {
            _shopsService = shopsService;
            _eventBus     = eventBus;
        }

        [HttpGet("shops")]
        public IActionResult GetAllShops()
        {
            _eventBus.Publish("Get Shops");
            return Ok(_shopsService.GetAllShops());
        }

        [HttpGet("shops/{id}")]
        public async Task<IActionResult> GetProducts(int shopId)
        {
            _eventBus.Publish("Get Products by shop");
            var products = await _shopsService.GetProductsByShop(shopId);
            return Ok(products);
        }

        [HttpPost("shops/{id]/find_by_category")]
        public async Task<IActionResult> GetProductsByCategory(int shopId, [FromBody] string categoryName)
        {
            _eventBus.Publish("Get Products by Category and Shop");
            if (!ModelState.IsValid) return BadRequest();
            var products = await _shopsService.GetProductsByCategory(shopId, categoryName);
            return Ok(products);
        }

        [Authorize]
        [HttpPost("shops/{id}/order")]
        public async Task<IActionResult> BuyProducts(int shopId, [FromBody] ICollection<Product> products)
        {
            _eventBus.Publish("Buy Products");
            if (!ModelState.IsValid) return BadRequest();
            var response = await _shopsService.BuyProducts(shopId, products);
            if (response == "Success")
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("updateProduct")]
        public async Task<IActionResult> AddProducts([FromBody] ICollection<ProductByFactory> products)
        {
            _eventBus.Publish("Add products from factory");
            if (!ModelState.IsValid) return BadRequest();
                await _shopsService.AddProductsByFactory(products);
            return Ok();
        }

    }
}
