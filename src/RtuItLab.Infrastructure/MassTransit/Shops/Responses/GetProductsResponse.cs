using System.Collections.Generic;
using RtuItLab.Infrastructure.Models.Shops;

namespace RtuItLab.Infrastructure.MassTransit.Shops.Responses
{
    public class GetProductsResponse
    {
        public bool Success { get; set; }
        public List<Product> Products { get; set; }
    }
}
