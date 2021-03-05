using System.Collections.Generic;
using RtuItLab.Infrastructure.Models.Shops;

namespace RtuItLab.Infrastructure.MassTransit.Shops.Responses
{
    public class GetAllShopsResponse
    {
        public bool Success { get; set; }
        public List<Shop> Shops { get; set; }
    }
}
