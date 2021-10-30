using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace BlazorCounterStream.Worker
{
    public class CounterBackgroundService : BackgroundService, IDisposable
    {
        private bool disposedValue;

        public Timer Timer { get; set; } = new(1000);
        public int Counter { get; set; } = 0;

        public event AsyncCounterEventHandler AsyncCounterEvent;

        public delegate Task AsyncCounterEventHandler(object sender, CounterEventArgs e);

        protected ILogger<CounterBackgroundService> Logger { get; set; }

        public CounterBackgroundService(ILogger<CounterBackgroundService> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
            return Task.CompletedTask;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Counter++;

            Logger.LogDebug($"Counter: {Counter}");
            AsyncCounterEventHandler handler = AsyncCounterEvent;
            if (handler != null)
                handler?.Invoke(this, new CounterEventArgs() { Counter = Counter });
            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Timer.Close();
                    Timer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CounterBackgroundService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }
    }
}
