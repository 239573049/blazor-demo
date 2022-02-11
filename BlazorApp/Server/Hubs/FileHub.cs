using Microsoft.AspNetCore.SignalR;

namespace BlazorApp.Server.Hubs
{
    public class FileHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
