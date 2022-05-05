using System.Collections.Concurrent;
using BlazorApp.Server.Jobs;
using Entitys.SSH;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace BlazorApp.Server.Hubs
{
    public class FileHub:Hub
    {
        public static BlockingCollection<string> ConnectIds=new();
        public static BlockingCollection<string> ServiceInfoIds=new();//需要推送服务器信息的用户id
        public override Task OnConnectedAsync()
        {
            ConnectIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var connect=Context.ConnectionId;
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
                if (QuartzFactory._scheduler!.GetTriggerState(ServerInfoJob._triggerKey!).ToString() != "Normal")
                {
                    await QuartzFactory._scheduler.ResumeTrigger(ServerInfoJob._triggerKey!);
                }
            }
            else
            {
                _ =ServiceInfoIds.TryTake(out connect);
                if (QuartzFactory._scheduler!.GetTriggerState(ServerInfoJob._triggerKey!).ToString() != "Paused"&& ServiceInfoIds.Count()==0)
                {
                    await QuartzFactory._scheduler.PauseTrigger(ServerInfoJob._triggerKey!);
                }
            }
            await base.Clients.Clients(connect!).SendAsync("ServiceInfoMessage", true);
        }
        public async Task SSHConnectSend(string json)
        {
            var sSHUser = JsonConvert.DeserializeObject<SSHUser>(json);
            await Clients.Clients(Context.ConnectionId).SendAsync("SSHConnectMessage",true);
        }
        public async Task SSHSend(string code)
        {

        }
    }
}
