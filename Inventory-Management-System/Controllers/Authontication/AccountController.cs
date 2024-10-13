using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Inventory_Management_System.Controllers.Authontication
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Register()
        {
            // Fetch roles from the database
            var roles = await roleManager.Roles.ToListAsync();

            // Create the RegisterViewModel and populate the Roles list
            var model = new RegisterViewModel
            {
                Roles = roles.Select(role => new SelectListItem
                {
                    Value = role.Name,
                    Text = role.Name
                }).ToList()
            };

            return View("Register", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRegister(RegisterViewModel registerViewModel)
        {

            if (ModelState.IsValid)
            {
                // Debugging/logging the Role value
                var selectedRole = registerViewModel.Role;
                if (string.IsNullOrEmpty(selectedRole))
                {
                    ModelState.AddModelError("", "Role is required.");
                    return View("Register", registerViewModel);
                }

                // Mapping
                ApplicationUser appUser = new ApplicationUser()
                {
                    UserName = registerViewModel.UserName,
                    PasswordHash = registerViewModel.Password,
                };

                // Save Data base
                IdentityResult result = await userManager.CreateAsync(appUser, registerViewModel.Password);
                if (result.Succeeded)
                {
                    // assign to role
                    await userManager.AddToRoleAsync(appUser, registerViewModel.Role);
                    
                    // Create Cookie
                    await signInManager.SignInAsync(appUser, false);
                    return RedirectToAction("Login", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View("Register", registerViewModel);
        }

        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveLogin(LoginUserViewModel loginUserViewModel)
        {
            if (ModelState.IsValid)
            {
                // check found
                ApplicationUser appUser = await userManager.FindByNameAsync(loginUserViewModel.UserName);
                if (appUser != null)
                {
                    // check his password
                    bool found = await userManager.CheckPasswordAsync(appUser, loginUserViewModel.Password);
                    if (found)
                    {
                        List<Claim> claims = new List<Claim>();
                        await signInManager.SignInWithClaimsAsync(appUser, loginUserViewModel.RememberMe, claims);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "UserName or Password are Wrong");
            }
            return View("Login", loginUserViewModel);
        }


        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return View("Login");
        }

    }
}
