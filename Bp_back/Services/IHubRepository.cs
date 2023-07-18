using Bp_back.Models.Buisness;

namespace Bp_back.Services
{
    public interface IHubRepository
    {
        void AddHub(string url);
        void RemoveHub(Guid id);
        void UpdateHub(Hub hub);
    }
}
