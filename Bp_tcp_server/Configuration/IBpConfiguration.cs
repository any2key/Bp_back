using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bp_tcp_server.Configuration
{
    internal interface IBpConfiguration
    {
        public int Port { get; set; }
        public int HttpPort { get; set; }
    }
}
