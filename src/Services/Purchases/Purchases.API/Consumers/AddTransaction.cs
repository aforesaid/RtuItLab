using MassTransit;
using Purchases.Domain.Services;
using RtuItLab.Infrastructure.MassTransit.Purchases.Requests;
using System.Threading.Tasks;

namespace Purchases.API.Consumers
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
