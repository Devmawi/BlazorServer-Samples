using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorCounterStream.Worker
{
    public class CounterMonitoringBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        protected ILogger<CounterMonitoringBackgroundService> Logger { get; set; }

        public CounterMonitoringBackgroundService(IServiceProvider serviceProvider, ILogger<CounterMonitoringBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            Logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var counterService = scope
                                        .ServiceProvider
                                            .GetRequiredService<CounterBackgroundService>();

                counterService.AsyncCounterEvent += CounterService_AsyncCounterEvent;
            }
            return Task.CompletedTask;
        }

        protected virtual async Task CounterService_AsyncCounterEvent(object sender, CounterEventArgs e)
        {
            Logger.LogDebug($"Monitored Value: {e.Counter} at {DateTime.UtcNow}");
            await Task.CompletedTask;
        }
    }
}
