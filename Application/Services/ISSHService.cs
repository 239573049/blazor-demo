using Renci.SshNet;

namespace Application.Services
{
    public interface ISSHService
    {
        /// <summary>
        /// 连接ssh
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        SshClient Connect(string host, int port,string username,string password);
    }
    public class SSHService:ISSHService
    {
        public SSHService()
        {

        }

        public SshClient Connect(string host, int port, string username, string password)
        {
            var sshClient = new SshClient(host, port, username, password);
            sshClient.Connect();
            return sshClient;
        }

    }
}
