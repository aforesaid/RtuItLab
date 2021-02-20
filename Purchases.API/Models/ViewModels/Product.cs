namespace Purchases.API.Models.ViewModels
{
    public class Product
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; } = 1;
        public string Category { get; set; }
    }
}
