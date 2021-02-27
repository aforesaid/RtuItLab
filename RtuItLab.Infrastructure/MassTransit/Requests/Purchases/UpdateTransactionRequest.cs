using ServicesDtoModels.Models.Identity;
using ServicesDtoModels.Models.Purchases;

namespace RtuItLab.Infrastructure.MassTransit.Requests.Purchases
{
    public class UpdateTransactionRequest
    {
        public User User { get; set; }
        public UpdateTransaction Transaction { get; set; }
    }
}
