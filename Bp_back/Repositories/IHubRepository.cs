using Bp_back.Models.Buisness;
using Bp_back.Models.Identity;

namespace Bp_back.Repositories
{
    public interface IHubRepository
    {
        void AddHub(string url,string name,int userId);
        void RemoveHub(Guid id);
        void UpdateHub(Hub hub);
        IEnumerable<Hub> GetHubs();
        IEnumerable<Server> GetHubServers(string url);
        IEnumerable<Server> GetHubServers(Guid id);
        public bool PingHub(string url);
        public Hub GetHub(Guid id);

    }
}
