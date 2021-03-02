using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Purchases.Domain.Services;
using RtuItLab.Infrastructure.MassTransit.Purchases.Responses;
using RtuItLab.Infrastructure.Models.Identity;

namespace RabbitMQ.Consumers.Purchases
{
    public class GetTransactions : PurchasesBaseConsumer, IConsumer<User>
    {
        public GetTransactions( IPurchasesService purchasesService) : base(purchasesService)
        {
            
        }
        public async Task Consume(ConsumeContext<User> context)
        {
            var order = await PurchasesService.GetTransactions(context.Message);
            await context.RespondAsync(new GetTransactionsResponse{
                Transactions = order.ToList(),
                Count = order.Count
            });
        }
    }
}
