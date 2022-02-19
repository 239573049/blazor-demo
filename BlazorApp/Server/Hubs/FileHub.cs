using BlazorApp.Server.Jobs;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using System.Collections.Concurrent;

namespace BlazorApp.Server.Hubs
{
    public class FileHub:Hub
    {
        public static BlockingCollection<string> ConnectIds=new();
        public static BlockingCollection<string> ServiceInfoIds=new();//需要推送服务器信息的用户id
        private readonly object _lock = new object();
        public override Task OnConnectedAsync()
        {
            ConnectIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string? connect=Context.ConnectionId;
            _ = ConnectIds.TryTake(out connect);
            _ = ServiceInfoIds.TryTake(out connect);
            return base.OnDisconnectedAsync(exception);
        }
        public async Task ServiceInfoSend(bool isStart)
        {
            var connect = Context.ConnectionId;
            if (isStart)
            {
                ServiceInfoIds.Add(connect);
                var c =await QuartzFactory._scheduler.GetJobDetail(new JobKey(ServerInfoJob._triggerKey.Name, ServerInfoJob._triggerKey.Group));
                if (QuartzFactory._scheduler.GetTriggerState(ServerInfoJob._triggerKey).ToString() != "Normal")
                {
                    await QuartzFactory._scheduler.ResumeTrigger(ServerInfoJob._triggerKey);
                }
            }
            else
            {
                _ =ServiceInfoIds.TryTake(out connect);
                if (QuartzFactory._scheduler.GetTriggerState(ServerInfoJob._triggerKey).ToString() != "Paused"&& ServiceInfoIds.Count()==0)
                {
                    await QuartzFactory._scheduler.PauseTrigger(ServerInfoJob._triggerKey);
                }
            }
            await base.Clients.Clients(connect!).SendAsync("ServiceInfoMessage", true);
        }
    }
}
