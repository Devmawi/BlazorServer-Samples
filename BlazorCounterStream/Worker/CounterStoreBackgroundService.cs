using BlazorCounterStream.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCounterStream.Worker
{
    public class CounterStoreBackgroundService : CounterMonitoringBackgroundService
    {
        private readonly CounterStore _counterStore;
        public CounterStoreBackgroundService(CounterStore counterStore,IServiceProvider serviceProvider, ILogger<CounterMonitoringBackgroundService> logger) : base(serviceProvider, logger)
        {
            _counterStore = counterStore;
        }

        protected override async Task CounterService_AsyncCounterEvent(object sender, CounterEventArgs e)
        {
            await Task.Delay(3000);
            _counterStore.AddValue(e.Counter);
            Logger.LogDebug($"Stored Value: {e.Counter} at {DateTime.UtcNow}");
            await Task.CompletedTask;
        }
    }
}
