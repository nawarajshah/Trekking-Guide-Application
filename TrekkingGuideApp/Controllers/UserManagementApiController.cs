using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrekkingGuideApp.Models;

namespace TrekkingGuideApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class UserManagementApiController : ControllerBase
    {
        private readonly UserManager<Users> _usersManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementApiController(UserManager<Users> usersManager, RoleManager<IdentityRole> roleManager)
        {
            _usersManager = usersManager;
            _roleManager = roleManager;
        }

        // GET: /api/usermanagementapi/users
        // Returns the list of manageable users
        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<IEnumerable<UserRolesDto>>> GetManageableUsers()
        {
            // Similar logic to your GetManageableUsersAsync
            var currentUser = await _usersManager.GetUserAsync(User);
            var currentRoles = await _usersManager.GetRolesAsync(currentUser);

            var allUsers = _usersManager.Users.ToList();
            var result = new List<UserRolesDto>();

            foreach (var user in allUsers) 
            {
                var roles = await _usersManager.GetRolesAsync(user);

                // Filter logic:
                // SuperAdmin can manage all except Admin & SuperAdmin.
                if (User.IsInRole("SuperAdmin"))
                {
                    if (!roles.Contains("SuperAdmin"))
                    {
                        result.Add(new UserRolesDto
                        {
                            UserId = user.Id,
                            Email = user.Email,
                            Roles = roles.ToList()
                        });
                    }
                }
                else if (User.IsInRole("Admin"))
                {
                    if (!roles.Contains("SuperAdmin") && !roles.Contains("Admin"))
                    {
                        result.Add(new UserRolesDto
                        {
                            UserId = user.Id,
                            Email = user.Email,
                            Roles = roles.ToList()
                        });
                    }
                }
            }

            return Ok(result);
        }

        // GET: /api/usermanagementapi/userform?userId=xxx
        // Returns the user info + available roles for editing
        [HttpGet]
        [Route("userform")]
        public async Task<ActionResult<EditUserRolesDto>> GetUserForm([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Ok(new EditUserRolesDto
                {
                    UserId = "",
                    Email = "",
                    SelectedRole = "",
                    AvailableRoles = new List<string>()
                });
            }

            var user = await _usersManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var userRoles = await _usersManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault() ?? "";

            // Filter roles based on the current logged-in user's role
            List<string> allowedRoles;
            if (User.IsInRole("SuperAdmin"))
                allowedRoles = new List<string> { "Admin", "User", "Guide" };
            else
                allowedRoles = new List<string> { "User", "Guide" };

            var allRoles = _roleManager.Roles
                .Where(r => allowedRoles.Contains(r.Name))
                .Select(r => r.Name)
                .ToList();

            var dto = new EditUserRolesDto
            {
                UserId = user.Id,
                Email = user.Email,
                SelectedRole = currentRole,
                AvailableRoles = allowedRoles
            };

            return Ok(dto);
        }

        // POST: /api/usermanagementapi/edit
        // Update the user's role
        [HttpPost]
        [Route("edit")]
        public async Task<ActionResult<IEnumerable<UserRolesDto>>> EditUserRole(EditUserRolesDto model)
        {
            var user = await _usersManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound("User not found");

            // Remove all current roles
            var currentManager = await _usersManager.GetRolesAsync(user);
            await _usersManager.RemoveFromRolesAsync(user, currentManager);

            // If a new role is selected, add it
            if (!string.IsNullOrEmpty(model.SelectedRole))
                await _usersManager.AddToRoleAsync(user, model.SelectedRole);

            // Return the updated user list
            return await GetManageableUsers();
        }

        // POST: /api/usermanagementapi/delete{userId}
        [HttpDelete]
        [Route("delete/{userId}")]
        public async Task<ActionResult<IEnumerable<UserRolesDto>>> DeleteUser(string userId)
        {
            var user = await _usersManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var result = await _usersManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest("Error deleting user.");

            // Return the updated user list
            return await GetManageableUsers();
        }
    }

    // Data transfer object for returning data to Angular
    public class UserRolesDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }

    public class EditUserRolesDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string SelectedRole { get; set; }
        public List<string> AvailableRoles { get; set; }
    }
}
