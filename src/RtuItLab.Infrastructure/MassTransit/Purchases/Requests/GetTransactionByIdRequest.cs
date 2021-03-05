using RtuItLab.Infrastructure.Models.Identity;

namespace RtuItLab.Infrastructure.MassTransit.Purchases.Requests
{
    public class GetTransactionByIdRequest
    {
        public User User { get; set; }
        public int Id { get; set; }
    }
}
