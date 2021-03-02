using RtuItLab.Infrastructure.Models.Identity;
using RtuItLab.Infrastructure.Models.Purchases;

namespace RtuItLab.Infrastructure.MassTransit.Purchases.Requests
{
    public class AddTransactionRequest
    {
        public User User { get; set; }
        public Transaction Transaction { get; set; }
    }
}
