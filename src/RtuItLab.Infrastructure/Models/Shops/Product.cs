using Newtonsoft.Json;

namespace RtuItLab.Infrastructure.Models.Shops
{
    public class Product
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; }
        public string Category { get; set; }
    }
}
