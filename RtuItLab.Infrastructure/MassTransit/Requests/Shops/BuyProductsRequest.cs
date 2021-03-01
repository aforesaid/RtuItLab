using ServicesDtoModels.Models.Shops;
using System.Collections.Generic;
using ServicesDtoModels.Models.Identity;

namespace RtuItLab.Infrastructure.MassTransit.Requests.Shops
{
    public class BuyProductsRequest
    {
        public User User { get; set; }
        public int ShopId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
