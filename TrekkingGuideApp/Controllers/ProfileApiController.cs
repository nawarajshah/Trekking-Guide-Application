using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrekkingGuideApp.Models;
using TrekkingGuideApp.ViewModels;

namespace TrekkingGuideApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileApiController : ControllerBase
    {
        private readonly UserManager<Users> _userManager;

        public ProfileApiController(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] ManageProfileViewModel model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            user.FullName = model.FullName;

            if (await _userManager.IsInRoleAsync(user, "Guide") || await _userManager.IsInRoleAsync(user, "Admin"))
            {
                user.Bio = model.Bio;
                user.Phone = model.Phone;
                user.Address = model.Address;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Profile updated successfully" });
        }

        // GET: /api/profileapi/roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            var profile = new ManageProfileViewModel
            {
                FullName = user.FullName,
                Bio = user.Bio,
                Phone = user.Phone,
                Address = user.Address
            };

            return Ok(profile);
        }
    }
}
