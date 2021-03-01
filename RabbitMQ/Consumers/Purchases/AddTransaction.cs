using MassTransit;
using Purchases.Domain.Services;
using RtuItLab.Infrastructure.MassTransit.Requests.Purchases;
using System.Threading.Tasks;

namespace RabbitMQ.Consumers.Purchases
{
    public class AddTransaction : PurchasesBaseConsumer, IConsumer<AddTransactionRequest>
    {
        public AddTransaction(IPurchasesService purchasesService) : base(purchasesService)
        {
        }
        public async Task Consume(ConsumeContext<AddTransactionRequest> context)
        {
            await PurchasesService.AddTransaction(context.Message.User, context.Message.Transaction);
            await context.RespondAsync(new AddTransactionResponse
            {
                Transaction = context.Message.Transaction,
                Success = true
            });
        }
    }
}
