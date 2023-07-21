using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bp_Hub.Controllers
{
    public class ControllerBaseEx : ControllerBase
    {
       

        protected IActionResult SafeRun(Func<string, object> action)
        {
            try
            {
                var resp = action("");
                return new JsonResult(resp);
            }
           
            catch (Exception ex)
            {
                return new JsonResult(Models.Responses.Response.BadResponse(ex.Message));
            }
        }

       
    }
}
