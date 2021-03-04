using MassTransit;
using Purchases.Domain.Services;
using RtuItLab.Infrastructure.MassTransit.Purchases.Requests;
using System.Threading.Tasks;

namespace RabbitMQ.Consumers.Purchases
{
    public class UpdateTransaction : PurchasesBaseConsumer, IConsumer<UpdateTransactionRequest>
    {
        public UpdateTransaction(IPurchasesService purchasesService) : base(purchasesService)
        {
        }
        public async Task Consume(ConsumeContext<UpdateTransactionRequest> context)
        {
           var order = await PurchasesService.UpdateTransaction(context.Message.User, context.Message.Transaction);
           await context.RespondAsync(order);
        }
    }
}
