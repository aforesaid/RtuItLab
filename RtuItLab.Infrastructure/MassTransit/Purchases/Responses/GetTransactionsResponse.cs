using System.Collections.Generic;
using RtuItLab.Infrastructure.Models.Purchases;

namespace RtuItLab.Infrastructure.MassTransit.Purchases.Responses
{
    public class GetTransactionsResponse
    {
        public List<Transaction> Transactions { get; set; }
        public int Count { get; set; }
    }
}
