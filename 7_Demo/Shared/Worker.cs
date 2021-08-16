using System;
using System.Threading;
using System.Threading.Tasks;
using Messages.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rebus.ServiceProvider;

namespace Messages
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _provider;
        private readonly ISubscribeService _subscribeService;

        public Worker(ILogger<Worker> logger, IServiceProvider provider, ISubscribeService subscribeService)
        {
            _logger = logger;
            _provider = provider;
            _subscribeService = subscribeService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _provider.UseRebus(async bus => { await _subscribeService.Subscribe(bus); });

            _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);
        }
    }
}