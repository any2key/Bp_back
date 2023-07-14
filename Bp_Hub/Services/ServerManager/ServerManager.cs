using Bp_Hub.Models;
using System.Diagnostics;
using System.IO;

namespace Bp_Hub.Services.ServerManager
{
    public class ServerManager : IServerManager
    {

        private readonly IConfiguration config;

        public int MaxServers = 5;
        private List<TcpServer> servers = new List<TcpServer>();
        public List<TcpServer> Servers { get => servers; }
        public List<int> PortPool = new List<int>();
        public ServerManager(IConfiguration config)
        {
            for (int i = 0; i < MaxServers; i++)
            {
                PortPool.Add(2708 + i);
            }

            this.config = config;
        }

        public void AddServer()
        {
            if (Servers.Count >= MaxServers)
                throw new Exception($"На хабе уже максимальное кол-во серверов");
            var port = PortPool.First();


            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo(config["ServerPath"], port.ToString())
                {
                    WindowStyle = ProcessWindowStyle.Maximized,
                    WorkingDirectory = Path.GetDirectoryName(config["ServerPath"])
                }
            };
           var res= process.Start();
            //Здесь запуск сервера
            PortPool.Remove(port);
        }

        public void RemoveServer(int port)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TcpServer> GetServers()
        {
            throw new NotImplementedException();
        }

        public TcpServer ServerInfo(int port)
        {
            throw new NotImplementedException();
        }
    }
}
