using Bp_back.Models.Buisness;
using Bp_back.Models.Responses;
using Bp_back.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

using ApiResponse = Bp_back.Models.Responses.Response;

namespace Bp_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBaseEx
    {

        private readonly IHubRepository hubRepository;

        public ServerController(IHubRepository hubRepository)
        {
            this.hubRepository = hubRepository;
        }

        [HttpPost]
        [Route("Fetch")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Fetch([FromBody] ServerFilter filter)
        {
            return SafeRun(_ =>
            {
                List<Server> servers = new List<Server>();
                var hubs = hubRepository.GetHubs();
                foreach (var hub in hubs)
                {
                    servers.AddRange(hubRepository.GetHubServers(hub.Id));
                }
                if (filter.HubId != null)
                    servers = servers.Where(e => e.Hub.Id == filter.HubId).ToList();
                if (filter.Active != null)
                    servers = servers.Where(e => e.Active == filter.Active).ToList();

                return new DataResponse<IEnumerable<Server>>() { Data = servers };
            });
        }


        [HttpGet]
        [Route("StopServer")]
        public async Task<IActionResult> StopServer(Guid hub, int port)
        {
            return SafeRun(_ =>
            {
                hubRepository.StopTcp(hub, port);
                return ApiResponse.OK;
            });
        }

        [HttpGet]
        [Route("StartServer")]
        public async Task<IActionResult> StartServer(Guid hub, int port)
        {
            return SafeRun(_ =>
            {
                hubRepository.StartTcp(hub, port);
                return ApiResponse.OK;
            });
        }

        [HttpGet]
        [Route("RemoveServer")]
        public async Task<IActionResult> RemoveServer(Guid hub, int port)
        {
            return SafeRun(_ =>
            {
                hubRepository.RemoveServer(hub, port);
                return ApiResponse.OK;
            });
        }
    }
}
