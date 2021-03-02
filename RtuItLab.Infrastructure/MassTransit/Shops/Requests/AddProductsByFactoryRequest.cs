using System.Collections.Generic;
using RtuItLab.Infrastructure.Models.Shops;

namespace RtuItLab.Infrastructure.MassTransit.Shops.Requests
{
    public class AddProductsByFactoryRequest
    {
        public List<ProductByFactory> Products { get; set; }
    }
}
