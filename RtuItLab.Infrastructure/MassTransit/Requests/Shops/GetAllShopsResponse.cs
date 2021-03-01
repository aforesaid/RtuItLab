using System.Collections.Generic;
using ServicesDtoModels.Models.Shops;

namespace RtuItLab.Infrastructure.MassTransit.Requests.Shops
{
    public class GetAllShopsResponse
    {
        public bool Success { get; set; }
        public List<Shop> Shops { get; set; }
    }
}
