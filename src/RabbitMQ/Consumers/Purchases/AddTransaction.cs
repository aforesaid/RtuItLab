using MassTransit;
using Purchases.Domain.Services;
using System.Threading.Tasks;
using RtuItLab.Infrastructure.MassTransit.Purchases.Requests;
using RtuItLab.Infrastructure.MassTransit.Purchases.Responses;

namespace RabbitMQ.Consumers.Purchases
{
    public class AddTransaction : PurchasesBaseConsumer, IConsumer<AddTransactionRequest>
    {
        public AddTransaction(IPurchasesService purchasesService) : base(purchasesService)
        {
        }
        public async Task Consume(ConsumeContext<AddTransactionRequest> context)
        {
           var order = await PurchasesService.AddTransaction(context.Message.User, context.Message.Transaction);
            await context.RespondAsync(order);
        }
    }
}
