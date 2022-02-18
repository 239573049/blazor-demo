using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace BlazorApp.Server.Hubs
{
    public class FileHub:Hub
    {
        public static BlockingCollection<string> ConnectIds=new();
        public override Task OnConnectedAsync()
        {
            ConnectIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string? connect=Context.ConnectionId;
            _ = ConnectIds.TryTake(out connect);
            return base.OnDisconnectedAsync(exception);
        }

    }
}
