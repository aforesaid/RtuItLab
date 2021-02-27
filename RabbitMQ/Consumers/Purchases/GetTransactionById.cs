using System.Threading.Tasks;
using MassTransit;
using Purchases.Domain.Services;
using RtuItLab.Infrastructure.MassTransit.Requests.Purchases;

namespace RabbitMQ.Consumers.Purchases
{
    public class GetTransactionById : PurchasesBaseConsumer, IConsumer<GetTransactionByIdRequest>
    {
        public GetTransactionById(IPurchasesService purchasesService) : base(purchasesService)
        {
            
        }

        public async Task Consume(ConsumeContext<GetTransactionByIdRequest> context)
        {
            var order = await PurchasesService.GetTransactionById(context.Message.User, context.Message.Id);
            await context.RespondAsync(order);
        }
    }
}
