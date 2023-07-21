using Bp_back.Models.Front;
using Bp_back.Models.Responses;
using Bp_back.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authorization;

using ApiResponse = Bp_back.Models.Responses.Response;


namespace Bp_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBaseEx
    {
        private readonly IDashboardRepository dashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository)
        {
            this.dashboardRepository = dashboardRepository;
        }

        [HttpGet]
        [Route("Hubs")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Hubs()
        {
            return SafeRun(_ =>
            {
                return new DataResponse<Dashboard>() { Data = dashboardRepository.GetDashboard() };
            });
        }
    }
}
