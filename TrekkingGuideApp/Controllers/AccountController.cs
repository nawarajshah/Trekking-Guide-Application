using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrekkingGuideApp.Models;
using TrekkingGuideApp.ViewModels;

namespace TrekkingGuideApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize] // all authenticated users can see this page
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // populate the view model with existing data
            var model = new ManageProfileViewModel
            {
                FullName = user.FullName,
                Bio = user.Bio,
                Phone = user.Phone,
                Address = user.Address
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(ManageProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // return the same view with validation errors
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // always allow updating Full name
            user.FullName = model.FullName;

            if (User.IsInRole("Guide") || User.IsInRole("Admin"))
            {
                user.Bio = model.Bio;
                user.Phone = model.Phone;
                user.Address = model.Address;
            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // handle errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            // optionally show a success message or redirect
            TempData["SuccessMessage"] = "Profile updated successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("NgApp", "Home");
                    //return RedirectToAction("NgApp", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                    return View(model);
                }
            }
            return View(model);
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                Users users = new Users
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(users, model.Password);

                if(result.Succeeded)
                {
                    // Assign the SuperAdmin role to this user
                    await userManager.AddToRoleAsync(users, "User");

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
