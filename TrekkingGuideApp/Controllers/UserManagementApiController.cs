using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TrekkingGuideApp.ViewModels;

namespace TrekkingGuideApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class UserManagementApiController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityUser> _roleManager;

        public UserManagementApiController(UserManager<IdentityUser> userManager, RoleManager<IdentityUser> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/UserManagementApi
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var result = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                // filtering logic as needed
                result.Add(new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }
            return Ok(result);
        }

        //// GET: api/UserManagementAPI/{userId}
        //[HttpGet{"userId}"]
        //public async Task<IActionResult> GetUser(string userId)
        //{
        //    if (string.IsNullOrEmpty(userId))
        //        return BadRequest();

        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //        return NotFound();


        //}

    }
}
