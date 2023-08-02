using Bp_Hub.Models;
using Bp_Hub.Models.Buisness;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using Bp_Hub.Services.Http;
using Bp_Hub.Models.Responses;
using System.Net.NetworkInformation;

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

        public ServerManager(IConfiguration config, IHttpService httpService)
        {

            this.config = config;
            this.httpService = httpService;
        }


        public void AddServer()
        {
            if (Servers.Count >= MaxServers)
                throw new Exception($"На хабе уже максимальное кол-во серверов");
            var port = GetFreePort();
            var httpPort = GetFreePort(port);


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
            servers.Add(new TcpServer() { Capacity = 100, Port = port, ProcessId = process.Id, HttpPort = httpPort });

        }


        public async void StartTcpListenAsync(int port)
        {
            try
            {

            var server = servers.FirstOrDefault(e => e.Port == port);
            await httpService.GetAsync<DataResponse<int>>($"http://localhost:{server.HttpPort}/startTcp");
            }
            catch { }
        }

        public async void StopTcpListenAsync(int port)
        {
            try
            {
                var server = servers.FirstOrDefault(e => e.Port == port);
                await httpService.GetAsync<DataResponse<int>>($"http://localhost:{server.HttpPort}/stopTcp");
            }
            catch { }
        }

        public void RemoveServer(int port)
        {
            var server = servers.First(e => e.Port == port);
            Process p = Process.GetProcessById(server.ProcessId);
            p.Kill();
            servers.Remove(server);
        }

        public IEnumerable<TcpServer> GetServers()
        {
            return servers;
        }

        public IEnumerable<Server> GetDTOServers(bool? active)
        {
            var totalServers = GetServers();
            List<Server> _servers = new List<Server>();
            Parallel.ForEach(totalServers, ts =>
            {
                _servers.Add(new Server()
                {
                    Id = ts.ProcessId,
                    PlayersCount = PlayerCount($"http://localhost:{ts.HttpPort}"),
                    Port = ts.Port,
                    HttpPort = ts.HttpPort,
                    Active = IsServerActive($"http://localhost:{ts.HttpPort}")
                });
            });

            if (active != null)
                _servers = _servers.Where(e => e.Active == active).ToList();

            return _servers;
        }


        private int PlayerCount(string url)
        {
            return httpService.GetAsync<DataResponse<int>>($"{url}/playersCount").Result.Data;
        }
        private bool IsServerActive(string url)
        {
            return httpService.GetAsync<DataResponse<bool>>($"{url}/isActive").Result.Data;

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
            RemoveAll();

        }

        public void RemoveAll()
        {
            foreach (var server in servers)
            {
                Process p = Process.GetProcessById(server.ProcessId);
                p.Kill();
            }
            servers.Clear();
        }


        private int GetFreePort(int busyPort = -1)
        {
            Random rnd = new Random();
            int port = rnd.Next(2000, 60000);
            if (busyPort == port)
                return GetFreePort();
            bool isAvailable = true;
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    isAvailable = false;
                    return GetFreePort();
                }
            }
            return port;
        }

        ~ServerManager()
        {
            RemoveAll();
        }
    }
}
