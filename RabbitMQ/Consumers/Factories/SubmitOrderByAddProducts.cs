using Factories.Domain.Services;
using MassTransit;
using RtuItLab.Infrastructure.MassTransit.Shops.Requests;
using Shops.Domain.Services;
using System.Threading.Tasks;

namespace RabbitMQ.Consumers.Factories
{
    public class SubmitOrderByAddProducts : IConsumer<AddProductsByFactoryRequest>
    {
        private readonly IFactoriesService _factoriesService;
        private readonly IShopsService _shopsService;

        public SubmitOrderByAddProducts(IFactoriesService factoriesService,
            IShopsService shopsService)
        {
            _factoriesService = factoriesService;
            _shopsService = shopsService;
        }

        public async Task Consume(ConsumeContext<AddProductsByFactoryRequest> context)
        {
            var order = await _factoriesService.CreateRequestInShops();
            await _shopsService.AddProductsByFactory(order);
        }
    }
}
