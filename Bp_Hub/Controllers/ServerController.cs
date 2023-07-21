using Bp_Hub.Models.Buisness;
using Bp_Hub.Models.Responses;
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
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            return SafeRun(_ =>
            {
                return new DataResponse<IEnumerable<Server>>() { Data= new List<Server>() { new Server() {Id=Guid.NewGuid(),Port=27 } } };
            });
        }
    }
}
