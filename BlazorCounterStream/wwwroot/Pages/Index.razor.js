/// <reference path="../lib/microsoft/signalr/dist/esm/browser-index.d.ts" />
export function startCounterHubConnection() {
    var hubConnection = new signalR
        .HubConnectionBuilder()
        .withUrl("/counterHub")
        .build();
    hubConnection.on("Count", param => {
        document.getElementById("counter").innerText = param;
        console.log(param);
    });
    //! Things to do: handle reconnect and so on
    hubConnection.start();
}
//# sourceMappingURL=Index.razor.js.map