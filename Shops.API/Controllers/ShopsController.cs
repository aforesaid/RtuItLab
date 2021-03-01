using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RtuItLab.Infrastructure.MassTransit.Requests.Shops;
using ServicesDtoModels.Models.Shops;
using Shops.API.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RtuItLab.Infrastructure.Models.Shops;
using ServicesDtoModels.Models.Identity;

namespace Shops.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IBusControl _busControl;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/shopsQueue");
        public ShopsController(IBusControl busControl)
        {
            _busControl = busControl;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShops()
        {
            var client = _busControl.CreateRequestClient<GetAllShopsRequest>(_rabbitMqUrl);
            var response = await client.GetResponse<GetAllShopsResponse>(new GetAllShopsRequest
            {
                User = new User()
            });
            return Ok(response.Message);
        }

        [HttpGet("{shopId}")]
        public async Task<IActionResult> GetProducts(int shopId)
        {
            var client = _busControl.CreateRequestClient<GetProductsRequest>(_rabbitMqUrl);
            var response = await client.GetResponse<GetProductsResponse>(new GetProductsRequest{
            ShopId = shopId
            });
            return Ok(response.Message);
        }

        [HttpPost("{shopId}/find_by_category")]
        public async Task<IActionResult> GetProductsByCategory(int shopId, [FromBody] Category category)
        {
            if (!ModelState.IsValid) return BadRequest();
            var client = _busControl.CreateRequestClient<GetProductsByCategoryRequest>(_rabbitMqUrl);
            var response = await client.GetResponse<GetProductsResponse>(new GetProductsByCategoryRequest
            {
                ShopId = shopId,
                Category = category.CategoryName
            });
            return Ok(response.Message);
        }

        [Authorize]
        [HttpPost("{shopId}/order")]
        public async Task<IActionResult> BuyProducts(int shopId, [FromBody] ICollection<Product> products)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = HttpContext.Items["User"] as User;
            var client = _busControl.CreateRequestClient<BuyProductsRequest>(_rabbitMqUrl);
            var response = await client.GetResponse<BuyProductsResponse>(new BuyProductsRequest
            {
                User = user,
                ShopId = shopId,
                Products = products
            }); 
            if (response.Message.Success)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }
    }
}
