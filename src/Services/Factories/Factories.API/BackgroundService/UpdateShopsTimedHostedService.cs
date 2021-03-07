using System;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Factories.Domain.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RtuItLab.Infrastructure.MassTransit.Shops.Requests;

namespace Factories.API.BackgroundService
{
    public class UpdateShopsTimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<UpdateShopsTimedHostedService> _logger;
        private readonly IBusControl _busControl;
        private readonly IServiceProvider _services;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/shopsQueue");

        private Timer _timer;
        private int _executionCount;
        private const int TimerSeconds = 20;
        public UpdateShopsTimedHostedService(ILogger<UpdateShopsTimedHostedService> logger,
            IBusControl busControl,
            IServiceProvider services)
        {
            _logger = logger;
            _busControl = busControl;
            _services = services;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Update Factories Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                               TimeSpan.FromSeconds(TimerSeconds));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref _executionCount);
            using var scope = _services.CreateScope();
            var factoriesService = scope.ServiceProvider.GetRequiredService<IFactoriesService>();
            var request = await factoriesService.CreateRequestInShops();
            var endpoint = await _busControl.GetSendEndpoint(_rabbitMqUrl);
            await endpoint.Send(new AddProductsByFactoryRequest{
                Products = request.ToList()
        });
            _logger.LogInformation(
                                   "Update Factories products", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Update Factories products is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
