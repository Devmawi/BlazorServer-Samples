using BlazorCounterStream.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCounterStream.Worker
{
    public class CounterSignalRBackgroundService : CounterMonitoringBackgroundService
    {
        private readonly IHubContext<CounterHub> _counterHub;
        public CounterSignalRBackgroundService(IHubContext<CounterHub> counterHub, IServiceProvider serviceProvider, ILogger<CounterMonitoringBackgroundService> logger) : base(serviceProvider, logger)
        {
            _counterHub = counterHub;
        }

        protected override async Task CounterService_AsyncCounterEvent(object sender, CounterEventArgs e)
        {
            await _counterHub.Clients.All.SendAsync("Count", e.Counter);
            Logger.LogDebug($"SignalR sends Value: {e.Counter} at {DateTime.UtcNow}");
            await Task.CompletedTask;
        }
    }
}
