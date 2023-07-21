using Bp_back.Models.Buisness;
using Bp_back.Models.Identity;
using Bp_back.Models.Responses;
using Bp_back.Services;
using System.Threading.Tasks;

namespace Bp_back.Repositories
{
    public class HubRepository : IHubRepository
    {
        private readonly IHttpService _httpService;
        public HubRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }


        public void AddHub(string url, User user)
        {
            BpEx.Run(db =>
            {
                if (db.Hubs.Any(e => e.Url.ToLower() == url.ToLower()))
                    throw new Exception($"Хаб с данным Url уже существует");

                db.Hubs.Add(new Hub() { Added = DateTime.Now, User = user });
                db.SaveChanges();
            });
        }

        public Hub GetHub(Guid id)
        {
            return BpEx.Run(db =>
            {
                var hub = db.Hubs.First(x => x.Id == id);
                hub.Active = PingHub(id);
                hub.Servers = GetHubServers(id);
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
                    hub.Active = PingHub(hub.Id);
                    hub.Servers = GetHubServers(hub.Id);
                });

                return hubs;
            }
            );
        }


        public IEnumerable<Server> GetHubServers(Guid id)
        {
            var hub = GetHub(id);
            return _httpService.GetAsync<DataResponse<IEnumerable<Server>>>($"{hub.Url}/Server/List").Result.Data;
        }

        public bool PingHub(Guid id)
        {
            var hub = GetHub(id);

            try { var res = _httpService.GetAsync<Response>($"{hub.Url}/Settings/Ping").Result; }
            catch { return false; }

            return true;
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
                var dbHub=db.Hubs.FirstOrDefault(e=>e.Id== hub.Id);
                dbHub.Url = hub.Url;
                db.SaveChanges();
            });
        }
    }
}
