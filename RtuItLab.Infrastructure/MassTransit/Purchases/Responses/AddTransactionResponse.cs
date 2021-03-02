using RtuItLab.Infrastructure.Models.Purchases;

namespace RtuItLab.Infrastructure.MassTransit.Purchases.Responses
{
    public class AddTransactionResponse
    {
        public bool Success { get; set; }
        public Transaction Transaction { get; set; }
    }
}
