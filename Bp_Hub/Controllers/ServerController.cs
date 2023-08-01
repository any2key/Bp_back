using Bp_Hub.Models.Buisness;
using Bp_Hub.Models.Responses;
using Bp_Hub.Services.ServerManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

using ApiResponse = Bp_Hub.Models.Responses.Response;
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

        [HttpGet]
        [Route("AddServer")]
        public async Task<IActionResult> AddServer()
        {
            return SafeRun(_ =>
            {
                serverManager.AddServer();
                return ApiResponse.OK;
            });
        }

        [HttpGet]
        [Route("RemoveAll")]
        public async Task<IActionResult> RemoveAll()
        {
            return SafeRun(_ =>
            {
                serverManager.RemoveAll();
                return ApiResponse.OK;
            });
        }
    }
}
