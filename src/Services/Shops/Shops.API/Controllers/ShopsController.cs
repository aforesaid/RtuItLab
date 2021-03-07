using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RtuItLab.Infrastructure.Filters;
using RtuItLab.Infrastructure.MassTransit;
using RtuItLab.Infrastructure.MassTransit.Shops.Requests;
using RtuItLab.Infrastructure.Models;
using RtuItLab.Infrastructure.Models.Identity;
using RtuItLab.Infrastructure.Models.Shops;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var response = await GetResponseRabbitTask<GetAllShopsRequest, ICollection<Shop>>(new GetAllShopsRequest());
            return Ok(ApiResult<ICollection<Shop>>.Success200(response));
        }

        [HttpGet("{shopId}")]
        public async Task<IActionResult> GetProducts(int shopId)
        {
            var response = await GetResponseRabbitTask<GetProductsRequest, ICollection<Product>>(new GetProductsRequest
            {
                ShopId = shopId,
            });
            return Ok(ApiResult<ICollection<Product>>.Success200(response));
        }

        [HttpPost("{shopId}/find_by_category")]
        public async Task<IActionResult> GetProductsByCategory(int shopId, [FromBody] Category category)
        {
            if (!ModelState.IsValid) return BadRequest();
            var response = await GetResponseRabbitTask<GetProductsByCategoryRequest, ICollection<Product>>(new GetProductsByCategoryRequest
            {
                ShopId = shopId,
                Category = category.CategoryName
            });
            return Ok(ApiResult<ICollection<Product>>.Success200(response));
        }

        [Authorize]
        [HttpPost("{shopId}/order")]
        public async Task<IActionResult> BuyProducts(int shopId, [FromBody] ICollection<Product> products)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = HttpContext.Items["User"] as User;
            var productsResponse = await GetResponseRabbitTask<BuyProductsRequest,ICollection<Product>>(new BuyProductsRequest()
            {
                User = user,
                ShopId = shopId,
                Products = products,
            });
            return Ok(ApiResult<ICollection<Product>>.Success200(products));
        }
        private async Task<TOut> GetResponseRabbitTask<TIn, TOut>(TIn request)
            where TIn : class
            where TOut : class
        {
            var client = _busControl.CreateRequestClient<TIn>(_rabbitMqUrl);
            var response = await client.GetResponse<ResponseMassTransit<TOut>>(request);
            return response.Message.Content ?? throw response.Message.Exception;
        }
    }
}
