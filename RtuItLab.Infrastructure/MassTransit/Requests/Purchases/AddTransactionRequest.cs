using ServicesDtoModels.Models.Identity;
using ServicesDtoModels.Models.Purchases;

namespace RtuItLab.Infrastructure.MassTransit.Requests.Purchases
{
    public class AddTransactionRequest
    {
        public User User { get; set; }
        public Transaction Transaction { get; set; }
    }
}
