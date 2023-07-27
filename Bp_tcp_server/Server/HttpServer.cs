using Bp_tcp_server.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bp_tcp_server.Server
{
    internal class HttpServer : IHttpServer
    {

        #region http_fields
        private Thread _serverThread;
        private HttpListener _httpListener;
        private HttpListenerContext _context;
        #endregion


        private readonly IBbServer bpServer;
        private readonly IBpConfiguration bpConfiguration;
        public bool Active => bpServer.Active;
        public HttpServer(IBbServer bpserver, IBpConfiguration bpConfiguration)
        {
            this.bpServer = bpserver;
            this.bpConfiguration = bpConfiguration;
        }
        public void Start()
        {
            _httpListener = new HttpListener();
            _serverThread = new Thread(Listen);
            _serverThread.Start();
        }

        public async void StartTcp()
        {
            await bpServer.Start();
        }

        public void StopTcp()
        {
            bpServer.Disconnect();
        }

        public void Listen()
        {
            var route = $"http://localhost:{bpConfiguration.HttpPort}";
            _httpListener.Prefixes.Add(route + "/");
            _httpListener.Start();
            bool listen = true;
            while (listen)
            {
                _context = _httpListener.GetContext();
                string content = string.Empty;
                string action = _context.Request.Url.AbsolutePath.Substring(1);

                switch (action)
                {
                    case "startTcp":
                        StartTcp();
                        break;
                    case "stopTcp":
                        StopTcp();
                        break;
                    case "stop":
                        listen = false;
                        break;
                    default: break;
                }

            }
        }
    }
}
