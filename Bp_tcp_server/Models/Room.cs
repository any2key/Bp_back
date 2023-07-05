using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bp_tcp_server.Client;
namespace Bp_tcp_server.Client;

internal class Room
{
    internal Guid Id { get; set; }
    internal List<Client> Clients { get; set; }
}
