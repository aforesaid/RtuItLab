using ServicesDtoModels.Models.Shops;
using System.Collections.Generic;

namespace RtuItLab.Infrastructure.MassTransit.Requests.Shops
{
    public class BuyProductsRequest
    {
        public int ShopId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
