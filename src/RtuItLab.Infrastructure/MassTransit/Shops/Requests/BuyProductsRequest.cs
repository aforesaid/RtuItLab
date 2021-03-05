using System.Collections.Generic;
using RtuItLab.Infrastructure.Models.Identity;
using RtuItLab.Infrastructure.Models.Shops;

namespace RtuItLab.Infrastructure.MassTransit.Shops.Requests
{
    public class BuyProductsRequest
    {
        public User User { get; set; }
        public int ShopId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
