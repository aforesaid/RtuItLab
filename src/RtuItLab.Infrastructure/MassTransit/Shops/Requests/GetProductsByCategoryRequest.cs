namespace RtuItLab.Infrastructure.MassTransit.Shops.Requests
{
    public class GetProductsByCategoryRequest
    {
        public int ShopId { get; set; }
        public string Category { get; set; }
    }
}
