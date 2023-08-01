using Bp_Hub.Models;
using Bp_Hub.Models.Buisness;

namespace Bp_Hub.Services.ServerManager
{
    public interface IServerManager
    {
        public List<TcpServer> Servers { get;}
        void AddServer();
        void RemoveServer(int port);
        IEnumerable<TcpServer> GetServers();
        TcpServer ServerInfo(int port);
        IEnumerable<Server> GetDTOServers();
        void StartTcpListen(int port) { }
        void RemoveAll();
    }
}
