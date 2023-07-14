using Bp_back.Utils;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bp_back.Controllers
{
    public class ControllerBaseEx : ControllerBase
    {
        protected int? UserID => int.Parse(FindClaim(ClaimTypes.NameIdentifier));
        protected string UserRole => FindClaim(ClaimTypes.Role);
        private string FindClaim(string claimName)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(claimName);
            if (claim == null)
            {
                return null;
            }
            return claim.Value;
        }

        protected IActionResult SafeRun(Func<string, object> action)
        {
            try
            {
                var resp = action("");
                return new JsonResult(resp);
            }
            catch (CodeException ex)
            {
                return Unauthorized("invalid_grant");
            }
            catch (Exception ex)
            {
                return new JsonResult(Models.Responses.Response.BadResponse(ex.Message));
            }
        }

        public class CodeException : Exception
        {
            public CodeException()
            {

            }
            public CodeException(string message)
                : base(message)
            {
            }
        }
    }
}
