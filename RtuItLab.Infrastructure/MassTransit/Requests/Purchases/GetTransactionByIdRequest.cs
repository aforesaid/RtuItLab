using ServicesDtoModels.Models.Identity;

namespace RtuItLab.Infrastructure.MassTransit.Requests.Purchases
{
    public class GetTransactionByIdRequest
    {
        public User User { get; set; }
        public int Id { get; set; }
    }
}
