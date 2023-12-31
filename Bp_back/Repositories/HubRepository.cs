﻿using Bp_back.Models.Buisness;
using Bp_back.Models.Identity;
using Bp_back.Models.Responses;
using Bp_back.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bp_back.Repositories
{
    public class HubRepository : IHubRepository
    {
        private readonly IHttpService _httpService;
        public HubRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }


        public void AddHub(string url, int port, string name, int userId)
        {
            BpEx.Run(db =>
            {
                if (db.Hubs.Any(e => e.Url.ToLower() == url.ToLower()))
                    throw new Exception($"Хаб с данным Url уже существует");
                var user = db.Users.First(e => e.Id == userId);
                db.Hubs.Add(new Hub() { Added = DateTime.Now, User = user, Url = url, Name = name, Port = port });
                db.SaveChanges();
            });
        }

        public void AddServer(Guid id)
        {
            Hub? hub = null;
            BpEx.Run(db =>
            {
                hub = db.Hubs.First(x => x.Id == id);
            });
            var response = _httpService.GetAsync<Response>($"{hub.Url}:{hub.Port}/api/server/addServer").Result;
            if (!response.IsOk)
                throw new Exception($"Что-то пошло не так");
        }


        public void StartTcp(Guid id,int port)
        {
            Hub? hub = null;
            BpEx.Run(db =>
            {
                hub = db.Hubs.First(x => x.Id == id);
            });
            var response = _httpService.GetAsync<Response>($"{hub.Url}:{hub.Port}/api/server/startServer?port={port}").Result;
            if (!response.IsOk)
                throw new Exception(response.Message);
        }

        public void StopTcp(Guid id, int port)
        {
            Hub? hub = null;
            BpEx.Run(db =>
            {
                hub = db.Hubs.First(x => x.Id == id);
            });
            var response = _httpService.GetAsync<Response>($"{hub.Url}:{hub.Port}/api/server/stopServer?port={port}").Result;
            if (!response.IsOk)
                throw new Exception(response.Message);

        }

        public void RemoveServer(Guid id, int port)
        {
            Hub? hub = null;
            BpEx.Run(db =>
            {
                hub = db.Hubs.First(x => x.Id == id);
            });
            var response = _httpService.GetAsync<Response>($"{hub.Url}:{hub.Port}/api/server/RemoveServer?port={port}").Result;
            if (!response.IsOk)
                throw new Exception(response.Message);
        }


        public Hub GetHub(Guid id)
        {
            return BpEx.Run(db =>
            {
                var hub = db.Hubs.First(x => x.Id == id);
                hub.Active = PingHub($"{hub.Url}:{hub.Port}");
                hub.Servers = GetHubServers($"{hub.Url}:{hub.Port}");
                return hub;
            }
            );
        }

        public IEnumerable<Hub> GetHubs()
        {
            return BpEx.Run(db =>
            {
                var hubs = db.Hubs.ToArray();
                Parallel.ForEach(hubs, (hub) =>
                {
                    hub.Active = PingHub($"{hub.Url}:{hub.Port}");
                    hub.Servers = hub.Active ? GetHubServers($"{hub.Url}:{hub.Port}") : new List<Server>();
                });

                return hubs;
            }
            );
        }


        public IEnumerable<Server> GetHubServers(string url)
        {
            try
            {
                return _httpService.GetAsync<DataResponse<IEnumerable<Server>>>($"{url}/api/Server/List").Result.Data;
            }
            catch
            {
                return new List<Server>();
            }
        }

        public IEnumerable<Server> GetHubServers(Guid id)
        {

            var hub = BpEx.Run(db => db.Hubs.First(e => e.Id == id));
            var _servers= GetHubServers($"{hub.Url}:{hub.Port}");
            foreach (var server in _servers)
                server.Hub = hub;
            return _servers;
        }

        public bool PingHub(string url)
        {

            try { var res = _httpService.GetAsync<Response>($"{url}/api/Settings/Ping").Result; }
            catch { return false; }

            return true;
        }

        public void RemoveAllServers(Guid id)
        {
            Hub? hub = null;
            BpEx.Run(db =>
            {
                hub = db.Hubs.First(x => x.Id == id);
            });
            var response = _httpService.GetAsync<Response>($"{hub.Url}:{hub.Port}/api/server/removeAll").Result;
            if (!response.IsOk)
                throw new Exception($"Что-то пошло не так");
        }

        public void RemoveHub(Guid id)
        {
            BpEx.Run(db =>
            {
                db.Hubs.Remove(db.Hubs.FirstOrDefault(e => e.Id == id));
                db.SaveChanges();
            });
        }

        public void UpdateHub(Hub hub)
        {
            BpEx.Run(db =>
            {
                var dbHub = db.Hubs.FirstOrDefault(e => e.Id == hub.Id);
                dbHub.Url = hub.Url;
                dbHub.Port = hub.Port;
                dbHub.Name = hub.Name;
                db.SaveChanges();
            });
        }
    }
}
