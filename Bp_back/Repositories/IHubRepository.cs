using Bp_back.Models.Buisness;
using Bp_back.Models.Identity;

namespace Bp_back.Repositories
{
    public interface IHubRepository
    {
        void AddHub(string url,User user);
        void RemoveHub(Guid id);
        void UpdateHub(Hub hub);
        IEnumerable<Hub> GetHubs();
        IEnumerable<Server> GetHubServers(Guid id);
        public bool PingHub(Guid id);
        public Hub GetHub(Guid id);

    }
}
