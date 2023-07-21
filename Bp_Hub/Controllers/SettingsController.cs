using Bp_Hub.Models.Buisness;
using Bp_Hub.Models.Responses;
using Microsoft.AspNetCore.Mvc;

using ApiResponse = Bp_Hub.Models.Responses.Response;

namespace Bp_Hub.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBaseEx
    {
        [HttpGet]
        [Route("Ping")]
        public async Task<IActionResult> Ping()
        {
            return SafeRun(_ =>
            {
                return ApiResponse.OK;
            });
        }
    }
}
