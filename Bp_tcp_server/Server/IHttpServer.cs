using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bp_tcp_server.Server
{
    public interface IHttpServer
    {
        bool Active { get; }
        void Start();
        void StartTcp();
        void StopTcp();
        void Listen();
    }
}
