using RtuItLab.Infrastructure.Models.Identity;
using RtuItLab.Infrastructure.Models.Purchases;

namespace RtuItLab.Infrastructure.MassTransit.Purchases.Requests
{
    public class UpdateTransactionRequest
    {
        public User User { get; set; }
        public UpdateTransaction Transaction { get; set; }
    }
}
