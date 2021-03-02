using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Factories.API.Services.BackgroundServices
{
    public class UpdateShopsTimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<UpdateShopsTimedHostedService> _logger;
        private readonly IFactoriesService _factoriesService;

        private Timer _timer;
        private int _executionCount;
        private const int TimerSeconds = 120;
        public UpdateShopsTimedHostedService(ILogger<UpdateShopsTimedHostedService> logger,
            IFactoriesService factoriesService)
        {
            _logger = logger;
            _factoriesService = factoriesService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Update Factories Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                               TimeSpan.FromSeconds(TimerSeconds));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref _executionCount);
            _factoriesService.CreateRequestInShops();
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
