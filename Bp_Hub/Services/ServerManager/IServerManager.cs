using Bp_Hub.Models;

namespace Bp_Hub.Services.ServerManager
{
    public interface IServerManager
    {
        public List<TcpServer> Servers { get;}
        void AddServer();
        void RemoveServer(int port);
        IEnumerable<TcpServer> GetServers();
        TcpServer ServerInfo(int port);
    }
}
