using Bp_tcp_server.Configuration;
using Bp_tcp_server.Models.Responses;
using Bp_tcp_server.Utils;
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

        public async Task StartTcp()
        {
            await bpServer.Start();
        }

        public void StopTcp()
        {
            bpServer.Disconnect();
        }

        public async void Listen()
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
                        RecieveOK();
                        break;
                    case "stopTcp":
                        StopTcp();
                        RecieveOK();
                        break;
                    case "stop":
                        listen = false;
                        StopTcp();
                        RecieveOK();
                        break;
                    case "playersCount":
                        RecieveData<int>(bpServer.PlayersCount);
                        break;
                    case "isActive":
                        RecieveData<bool>(bpServer.Active);
                        break;
                    default: break;
                }

            }
        }


        private void RecieveOK() => SendContent(Encoding.UTF8.GetBytes(Response.OK.ToJson()));

        private void RecieveData<T>(T data) => SendContent(Encoding.UTF8.GetBytes(new DataResponse<T>() { Data = data }.ToJson()));


        private void SendContent(byte[] bytes)
        {
            try
            {
                using Stream stream = new MemoryStream(bytes);
                _context.Response.ContentType = "application/json";
                _context.Response.ContentLength64 = stream.Length;
                byte[] buffer = new byte[64 * 1024];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    _context.Response.OutputStream.Write(buffer, 0, bytesRead);
            }
            catch (Exception ex)
            {
                _context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Console.WriteLine(ex.Message);
                _context.Response.OutputStream.Close();
            }
        }
    }
}
