using BlazorApp.Server.Hubs;
using Entitys.Web;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Quartz;
using System.Runtime.InteropServices;
using Utils;

namespace BlazorApp.Server.Jobs
{
    public class ServerInfoJob : IJob
    {
        IHubContext<FileHub> _fileHub;
        private readonly object _lock = new object();
        public ServerInfoJob(
            IHubContext<FileHub> fileHub
            )
        {
            _fileHub = fileHub;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var data = new ServiceInfoModel();
            lock (_lock)
            {
                data.SystemOs = RuntimeInformation.OSDescription;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    data.Available = WinUtil.GetRAM();
                    data.SystemUpTime = WinUtil.GetSystemUpTime();
                    data.Total = WinUtil.GetMemory();
                    data.Cpu = WinUtil.GetCpuUsage();
                    data.Usage = Convert.ToInt32((data.Total - data.Available) / data.Total * 100);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    var use = LinuxUtil.ReadMemInfo();
                    data.Available = use.Available;
                    data.Total = use.Total;
                    data.Usage = use.Usage;
                    data.Cpu = LinuxUtil.QUERY_CPULOAD(false);
                }
            }
            await _fileHub.Clients.Clients(FileHub.ConnectIds).SendAsync("ServiceInfo", JsonConvert.SerializeObject(data));
        }
    }
}
