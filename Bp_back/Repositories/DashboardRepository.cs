using Bp_back.Models.Buisness;
using Bp_back.Models.Front;
using Bp_back.Utils;

namespace Bp_back.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IHubRepository hubRepository;
        public DashboardRepository(IHubRepository hubRepository)
        {
            this.hubRepository = hubRepository;
        }

        public Dashboard GetDashboard()
        {
            Dashboard dashboard = new Dashboard();
            return dashboard.BuildDashBoard(hubRepository);
        }
    }
}
