using Bp_back.Models.Buisness;
using Bp_back.Models.Front;
using Bp_back.Repositories;

namespace Bp_back.Utils
{
    public static class DataExt
    {
        public static Dashboard BuildDashBoard(this Dashboard dashboard, IHubRepository hubRepository)
        {
            IEnumerable<Hub> hubs = hubRepository.GetHubs();
            dashboard.TotalHubs = hubs.Count();

            foreach (var hub in hubs)
            {
                dashboard.Servers += hub.Servers.Count();
            }
            dashboard.OnHubs = hubs.Where(e => e.Active).Count();

            return dashboard;
        }
    }
}
