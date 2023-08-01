using Bp_Hub.Models;
using Bp_Hub.Models.Buisness;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using Bp_Hub.Services.Http;
using Bp_Hub.Models.Responses;

namespace Bp_Hub.Services.ServerManager
{
    public class ServerManager : IServerManager, IDisposable
    {

        private readonly IConfiguration config;
        private readonly IHttpService httpService;

        bool isDisposed = false;

        public int MaxServers = 5;
        private List<TcpServer> servers = new List<TcpServer>();
        public List<TcpServer> Servers { get => servers; }
        public List<int> PortPool = new List<int>();
        public List<int> HttpPortPool = new List<int>();
        public ServerManager(IConfiguration config, IHttpService httpService)
        {
            for (int i = 0; i < MaxServers; i++)
            {
                PortPool.Add(2708 + i);
            }
            for (int i = 0; i < MaxServers; i++)
            {
                HttpPortPool.Add(3708 + i);
            }

            this.config = config;
            this.httpService = httpService;
        }


        public void AddServer()
        {
            if (Servers.Count >= MaxServers)
                throw new Exception($"На хабе уже максимальное кол-во серверов");
            var port = PortPool.First();
            var httpPort = HttpPortPool.First();


            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo(config["ServerPath"], $"{port.ToString()} {httpPort.ToString()}")
                {
                    WindowStyle = ProcessWindowStyle.Maximized,
                    WorkingDirectory = Path.GetDirectoryName(config["ServerPath"]),

                }
            };
            var res = process.Start();
            //Здесь запуск сервера
            PortPool.Remove(port);
            servers.Add(new TcpServer() { Capacity = 100, Port = port, ProcessId = process.Id, HttpPort = httpPort });
        }

        public void StartTcpListen(int port) { }

        public void RemoveServer(int port)
        {
            foreach (var server in servers)
            {
                try
                {
                    Process p = Process.GetProcessById(server.ProcessId);
                    p.Kill();
                }
                catch
                {
                    continue;
                }
            }
            //Разорвать соединение, убить процесс
        }

        public IEnumerable<TcpServer> GetServers()
        {
            return servers;
        }

        public IEnumerable<Server> GetDTOServers()
        {
            var totalServers = GetServers();
            List<Server> servers = new List<Server>();
            Parallel.ForEach(totalServers, ts =>
            {
                servers.Add(new Server()
                {
                    Id = ts.ProcessId,
                    PlayersCount = PlayerCount($"http://localhost:{ts.HttpPort}"),
                    Port = ts.Port
                });
            });

            return servers;
        }


        private int PlayerCount(string url)
        {
            return httpService.GetAsync<DataResponse<int>>($"{url}/playersCount").Result.Data;
        }

        public TcpServer ServerInfo(int port)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (isDisposed)
                return;
            isDisposed = true;
            foreach (var server in servers)
            {
                RemoveServer(server.Port);
            }

        }
        ~ServerManager()
        {
            foreach (var server in servers)
            {
                RemoveServer(server.Port);
            }
        }
    }
}
