using System.Collections.Generic;
using ServicesDtoModels.Models.Purchases;

namespace RtuItLab.Infrastructure.MassTransit.Requests.Purchases
{
    public class GetTransactionsResponse
    {
        public List<Transaction> Transactions { get; set; }
        public int Count { get; set; }
    }
}
