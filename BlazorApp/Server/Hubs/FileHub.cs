using Microsoft.AspNetCore.SignalR;

namespace BlazorApp.Server.Hubs
{
    public class FileHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            var conn=Context.GetHttpContext();
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
