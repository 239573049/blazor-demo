using System.Diagnostics;
using System.Management;

namespace Utils
{
    public class WinUtil
    {
        static readonly PerformanceCounter cpuCounter = new("Processor", "% Processor Time", "_Total");
#pragma warning disable CA1416 // 验证平台兼容性
        static readonly PerformanceCounter ramCounter = new("Memory", "Available MBytes");
#pragma warning restore CA1416 // 验证平台兼容性
#pragma warning disable CA1416 // 验证平台兼容性
        static readonly PerformanceCounter uptime = new("System", "System Up Time");


        public static bool GetInternetAvilable()
        {
            bool networkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            return networkUp;
        }

        public static TimeSpan GetSystemUpTime()
        {
            uptime.NextValue();
            TimeSpan ts = TimeSpan.FromSeconds(uptime.NextValue());
            return ts;
        }

        public static long GetMemory()
        {
            string? str = null;
            var objCS = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject objMgmt in objCS.Get())
            {
                str = objMgmt["totalphysicalmemory"].ToString();
            }
            return Convert.ToInt64(str) / (1024 * 1024);
        }

        public static int GetCpuUsage()
        {
            return Convert.ToInt32(cpuCounter.NextValue());
        }

        public static int GetRAM()
        {
            return Convert.ToInt32(ramCounter.NextValue());
        }
    }
}
