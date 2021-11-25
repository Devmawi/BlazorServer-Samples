export function startSignalRClient() {
    var hubConnection = new signalR
        .HubConnectionBuilder()
        .withUrl("/videoStreamHub")
        .build();

    hubConnection.on("NewFrame", param => {
        document.querySelector("img").src = `data:image/png;base64,${param}`
        console.log(param)
    })
    //! Things to do: handle reconnect and so on
    hubConnection.start();
}