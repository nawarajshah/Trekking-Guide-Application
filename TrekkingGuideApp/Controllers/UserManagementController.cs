using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrekkingGuideApp.Models;
using TrekkingGuideApp.ViewModels;

namespace TrekkingGuideApp.Controllers
{
    // this controller is accessible to users in the admin or superadmin roles
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private async Task<List<UserRolesViewModel>> GetManageableUsersAsync()
        {
            // Get list of manageable users based on the current user's role.
            // for example:
            var currentUser = await _userManager.GetUserAsync(User);
            var currentRoles = await _userManager.GetRolesAsync(currentUser);

            // fetch all users and filter out those not manageable by current user.
            var allUsers = _userManager.Users.ToList();
            var manageableUsers = new List<UserRolesViewModel>();

            foreach (var user in allUsers)
            {
                // get roles for the user
                var roles = await _userManager.GetRolesAsync(user);
                // example filtering:
                // superadmin can manage all except other superadmins.
                // admin can manage all except users with roles admin or superadmin.
                if (User.IsInRole("SuperAdmin"))
                {
                    if (!roles.Contains("SuperAdmin"))
                    {
                        manageableUsers.Add(new UserRolesViewModel
                        {
                            UserId = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            Roles = roles.ToList()
                        });
                    }
                }
                else if (User.IsInRole("Admin"))
                {
                    if (!roles.Contains("SuperAdmin") && !roles.Contains("Admin"))
                    {
                        manageableUsers.Add(new UserRolesViewModel
                        {
                            UserId = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            Roles = roles.ToList()
                        });
                    }
                }
            }

            return manageableUsers;
        }

        // Main page: shows both user list and (initially empty) form.
        public async Task<IActionResult> Index()
        {

            var model = await GetManageableUsersAsync();
            return View(model);
        }


        // ajax: return the user edit form populated for the selected user.
        [HttpGet]
        public async Task<IActionResult> GetUserForm(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                // return an empty form partial
                return PartialView("_UserFormPartial", new EditUserRolesViewModel 
                { 
                    AvailableRoles = new List<SelectListItem>()  
                });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return PartialView("_UserFormPartial", new EditUserRolesViewModel 
                { 
                    AvailableRoles = new List<SelectListItem>() 
                });
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault() ?? "";

            // get all roles as available options.
            var allRoles = _roleManager.Roles.ToList();
            List<string> allowedRoles = new List<string>();

            // filter available roles based on the current logged-in user's role
            if (User.IsInRole("SuperAdmin"))
            {
                // superadmin can only assign admin, user, and guide
                allowedRoles = new List<string> { "Admin", "User", "Guide" };
            }
            else if (User.IsInRole("Admin"))
            {
                // admin can only user and guide
                allowedRoles = new List<string> { "User", "Guide" };
            }

            // build the selectlist from filtered roles
            var availableRoles = allRoles
                .Where(r => allowedRoles.Contains(r.Name))
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name,
                    Selected = (r.Name == currentRole)
                }).ToList();

            var model = new EditUserRolesViewModel
            {
                UserId = userId,
                Email = user.Email,
                SelectedRole = currentRole,
                AvailableRoles = availableRoles
            };

            return PartialView("_UserFormPartial", model);
        }
       

        // POST: /UserManagement/Edit update the selected user's role
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserRolesViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_UserFormPartial", model);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) 
                return NotFound();

            // remove all current roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // if a new role is selected, add it
            if (!string.IsNullOrEmpty(model.SelectedRole))
            {
                await _userManager.AddToRoleAsync(user, model.SelectedRole);
            }

            var updatedUserList = await GetManageableUsersAsync();
            return PartialView("_UserListPartial", updatedUserList);
        }

        // POST: /UserManagement/DeleteUser/{id}
        [HttpPost, ActionName("DeleteUser")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Delete(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                TempData["Error"] = "Error deleting user.";

            var updatedUserList = await GetManageableUsersAsync();
            return PartialView("_UserListPartial", updatedUserList);
        }
    }
}
