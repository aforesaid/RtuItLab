using RtuItLab.Infrastructure.Models.Identity;

namespace RtuItLab.Infrastructure.MassTransit.Shops.Requests
{
    public class GetAllShopsRequest
    {
        public User User { get; set; }
    }
}
