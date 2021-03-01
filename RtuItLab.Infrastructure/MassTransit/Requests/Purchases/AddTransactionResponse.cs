using System.Collections.Generic;
using RtuItLab.Infrastructure.Models.Purchases;

namespace RtuItLab.Infrastructure.MassTransit.Requests.Purchases
{
    public class AddTransactionResponse
    {
        public bool Success { get; set; }
        public Transaction Transaction { get; set; }
    }
}
