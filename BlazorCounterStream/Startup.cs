using BlazorCounterStream.Data;
using BlazorCounterStream.Hubs;
using BlazorCounterStream.Services;
using BlazorCounterStream.Worker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCounterStream
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSignalR();
         
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            
            services.AddSingleton<CounterBackgroundService>();
            services.AddHostedService(s => s.GetRequiredService<CounterBackgroundService>());

            services.AddSingleton<CounterMonitoringBackgroundService>();
            services.AddHostedService(s => s.GetRequiredService<CounterMonitoringBackgroundService>());

            services.AddSingleton<CounterStoreBackgroundService>();
            services.AddHostedService(s => s.GetRequiredService<CounterStoreBackgroundService>());

            services.AddSingleton<CounterSignalRBackgroundService>();
            services.AddHostedService(s => s.GetRequiredService<CounterSignalRBackgroundService>());

            services.AddSingleton<CounterStore>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHub<CounterHub>("/counterHub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
