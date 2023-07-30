using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bp_tcp_server.Server
{
    public interface IBbServer
    {
        bool Active { get; }
        Task Start();
        void Disconnect();
    }
}
