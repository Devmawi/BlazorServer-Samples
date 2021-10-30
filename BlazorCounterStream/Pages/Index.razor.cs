using BlazorCounterStream.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorCounterStream.Pages
{
    public partial class Index
    {
        [Inject]
        public CounterStore CounterStore { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private IJSObjectReference module;

        public Timer Timer { get; set; } = new(1000);

        public int LastCounter { get; set; }

        protected override Task OnInitializedAsync()
        {
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // implement like this https://docs.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-javascript-from-dotnet?view=aspnetcore-5.0#javascript-isolation-in-javascript-modules-1
                module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Index.razor.js");
                await module.InvokeVoidAsync("startCounterHubConnection");
            }
                
            await base.OnAfterRenderAsync(firstRender);      
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await InvokeAsync(() =>
            {
                LastCounter = CounterStore.CounterHistory.Any() ? CounterStore.CounterHistory.Last() : 0;
                StateHasChanged();
            });
        }
    }
}
