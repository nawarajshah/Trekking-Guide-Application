using Microsoft.AspNetCore.Identity;
using TrekkingGuideApp.Models;

namespace TrekkingGuideApp.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            // Retrieve the RoleManager and UserManager from the DI container.
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();

            // define the role for our system
            string[] roles = new string[] { "SuperAdmin", "Admin", "Guide", "User" };

            // loop through each role and create it if does not already exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Now seed the SuperAdmin user if it doesn't exist
            string superAdminEmail = "superadmin@example.com";
            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);
            if (superAdmin == null)
            {
                superAdmin = new Users
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    FullName = "Super Admin"
                };

                // Create the superadmin user with a strong password
                var result = await userManager.CreateAsync(superAdmin, "SuperAdmin@123");
                if (!result.Succeeded)
                {
                    // log or throw an exception with the errors
                    throw new Exception("Failed to create SuperAdmin: " + 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                // Assign the SuperAdmin role to this user
                await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
            }
        }
    }
}
