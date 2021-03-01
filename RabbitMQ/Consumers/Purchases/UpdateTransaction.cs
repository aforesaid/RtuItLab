using MassTransit;
using Purchases.Domain.Services;
using RtuItLab.Infrastructure.MassTransit.Requests.Purchases;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RtuItLab.Infrastructure.MassTransit.Responds.Purchases;

namespace RabbitMQ.Consumers.Purchases
{
    public class UpdateTransaction : PurchasesBaseConsumer, IConsumer<UpdateTransactionRequest>
    {
        private readonly ILogger<UpdateTransaction> _logger;
        public UpdateTransaction(IPurchasesService purchasesService,ILogger<UpdateTransaction> logger) : base(purchasesService)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<UpdateTransactionRequest> context)
        {
           var (content, isSuccess) = await PurchasesService.UpdateTransaction(context.Message.User, context.Message.Transaction);
           _logger.LogInformation($"content:{content} isSuccess:{isSuccess}");
           await context.RespondAsync(new UpdateTransactionRespond
           {
               Content = content,
               Success = isSuccess
           });
        }
    }
}
