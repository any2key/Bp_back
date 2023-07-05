using Bp_tcp_server.Configuration;
using Bp_tcp_server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bp_tcp_server.Server
{
    internal class BpServer : IBbServer
    {
        private IBpLogger logger;
        private TcpListener tcpListener;
        private readonly IBpConfiguration config;
        public BpServer(IBpLogger logger, IBpConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }

        async Task IBbServer.Start()
        {
            try
            {

                tcpListener = new TcpListener(IPAddress.Any, config.Port);
                tcpListener.Start();
                logger.Log($"Server Started...");
                
                while (true)
                {
                    TcpClient client = await tcpListener.AcceptTcpClientAsync();
                    Task.Run(() => { });
                }
            }
            finally
            {
                tcpListener.Stop();
            }
        }


    }
}
