using MassTransit;
using Purchases.Domain.Services;
using RtuItLab.Infrastructure.MassTransit.Requests.Purchases;
using System.Threading.Tasks;
using RtuItLab.Infrastructure.MassTransit.Responds.Purchases;

namespace RabbitMQ.Consumers.Purchases
{
    public class UpdateTransaction : PurchasesBaseConsumer, IConsumer<UpdateTransactionRequest>
    {
        public UpdateTransaction(IPurchasesService purchasesService) : base(purchasesService)
        {
        }
        public async Task Consume(ConsumeContext<UpdateTransactionRequest> context)
        {
           var (content, isSuccess) = await PurchasesService.UpdateTransaction(context.Message.User, context.Message.Transaction);
           await context.RespondAsync(new UpdateTransactionRespond
           {
               Content = content,
               IsSuccess = isSuccess
           });
        }
    }
}
