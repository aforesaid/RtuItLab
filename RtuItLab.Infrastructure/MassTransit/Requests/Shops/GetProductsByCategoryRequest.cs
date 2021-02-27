namespace RtuItLab.Infrastructure.MassTransit.Requests.Shops
{
    public class GetProductsByCategoryRequest
    {
        public int ShopId { get; set; }
        public string Category { get; set; }
    }
}
