using Bp_back.Models.Buisness;
using Bp_back.Models.Responses;
using Microsoft.AspNetCore.Mvc;

using ApiResponse = Bp_back.Models.Responses.Response;

namespace Bp_back.Controllers
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
