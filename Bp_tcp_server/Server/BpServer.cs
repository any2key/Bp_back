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
    public class BpServer : IBbServer
    {
        private IBpLogger logger;
        private TcpListener tcpListener;
        private readonly IBpConfiguration config;

        private List<Connection> connections = new List<Connection>();

        private List<Room> Rooms = new List<Room>();

        private bool active;
        public bool Active { get => active; }

        public int PlayersCount => connections?.Count ?? 0;

        public BpServer(IBpLogger logger, IBpConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }


        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            Connection? connection = connections.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (connection != null) connections.Remove(connection);
            connection?.Close();
        }

        protected internal async Task BroadcastMessageAsync(string message, string id)
        {
            foreach (var connection in connections)
            {
                if (connection.Id != id) // если id клиента не равно id отправителя
                {
                    await connection.Writer.WriteLineAsync(message); //передача данных
                    await connection.Writer.FlushAsync();
                }
            }
        }


        async Task IBbServer.Start()
        {
            try
            {

                tcpListener = new TcpListener(IPAddress.Any, config.Port);
                tcpListener.Start();
                logger.Log($"Server Started...");
                active = true;
                await Task.Run(async () =>
                {
                });
                while (true)
                {
                    TcpClient client = await tcpListener.AcceptTcpClientAsync();
                    Connection connection = new Connection(client, this);
                    connections.Add(connection);
                    Task.Run(connection.ProcessAsync);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            finally
            {
                Disconnect();
            }

        }

        public void Disconnect()
        {
            foreach (var connection in connections)
            {
                connection.Close(); //отключение клиента
            }
            tcpListener.Stop(); //остановка сервера
            active = false;
        }


        public class Connection
        {
            public string Id { get; set; }
            protected internal StreamWriter Writer { get; }
            protected internal StreamReader Reader { get; }

            TcpClient client;
            BpServer server;

            public Connection(TcpClient client, BpServer server)
            {
                this.client = client;
                this.server = server;

                var stream = client.GetStream();
                Reader = new StreamReader(stream);
                Writer = new StreamWriter(stream);
            }


            public async Task ProcessAsync()
            {
                try
                {
                    string? message = await Reader.ReadLineAsync();
                    await server.BroadcastMessageAsync(message, Id);
                    Console.WriteLine(message);
                    while (true)
                    {
                        try
                        {
                            message = await Reader.ReadLineAsync();
                            if (message == null) continue;
                            Console.WriteLine(message);
                            await server.BroadcastMessageAsync(message, Id);
                        }
                        catch
                        {
                            message = $"Отключился";
                            Console.WriteLine(message);
                            await server.BroadcastMessageAsync(message, Id);
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    // в случае выхода из цикла закрываем ресурсы
                    server.RemoveConnection(Id);
                }
            }
            protected internal void Close()
            {
                Writer.Close();
                Reader.Close();
                client.Close();
            }


        }
    }
}
