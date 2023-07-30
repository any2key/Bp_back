using Bp_Hub.Models.Buisness;
using Bp_Hub.Models.Responses;
using Bp_Hub.Services.ServerManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Bp_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBaseEx
    {
        private readonly IServerManager serverManager;
        public ServerController(IServerManager serverManager)
        {
            this.serverManager = serverManager;
        }
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            return SafeRun(_ =>
            {
                return new DataResponse<IEnumerable<Server>>() { Data = serverManager.GetDTOServers() };
            });
        }
    }
}
