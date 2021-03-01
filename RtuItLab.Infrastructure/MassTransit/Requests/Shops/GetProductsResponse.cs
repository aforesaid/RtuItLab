using System.Collections.Generic;
using ServicesDtoModels.Models.Shops;

namespace RtuItLab.Infrastructure.MassTransit.Requests.Shops
{
    public class GetProductsResponse
    {
        public bool Success { get; set; }
        public List<Product> Products { get; set; }
    }
}
