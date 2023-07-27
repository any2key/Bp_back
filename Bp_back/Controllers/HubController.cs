using Bp_back.Models.Buisness;
using Bp_back.Models.Front;
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
    public class HubController : ControllerBaseEx
    {
        private readonly IHubRepository hubRepository;
        public HubController(IHubRepository hubRepository)
        {
            this.hubRepository = hubRepository;
        }

        [HttpGet]
        [Route("AddHub")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddHub(string url, int port, string name)
        {
            return SafeRun(_ =>
            {
                hubRepository.AddHub(url, port, name, UserID.Value);
                return ApiResponse.OK;
            });
        }

        [HttpGet]
        [Route("Hubs")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Hubs(Guid id)
        {
            return SafeRun(_ => new DataResponse<IEnumerable<Hub>>() { Data = hubRepository.GetHubs() });
        }

        [HttpGet]
        [Route("Servers")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Servers(Guid id)
        {
            return SafeRun(_ => new DataResponse<IEnumerable<Server>>() { Data = hubRepository.GetHubServers(id) });
        }

        [HttpPost]
        [Route("UpdateHub")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateHub([FromBody] Hub hub)
        {
            return SafeRun(_ =>
            {
                hubRepository.UpdateHub(hub);
                return ApiResponse.OK;
            });
        }
    }
}
