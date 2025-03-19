using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TrekkingGuideApp.Controllers
{
    public class AccountApiController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("api/account/logout")]
        public async Task<IActionResult> LogoutApi()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
