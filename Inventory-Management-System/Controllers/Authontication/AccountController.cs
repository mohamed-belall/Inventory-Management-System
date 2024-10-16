using Inventory_Management_System.Repository;
using Inventory_Management_System.Repository.repo;
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
        private readonly IEmployeeRepository employeeRepository;

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                RoleManager<IdentityRole> roleManager ,
                                IEmployeeRepository employeeRepository)

        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.employeeRepository = employeeRepository;
        }
        /**************************** Register ****************************/
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
                    UserName = registerViewModel.FName +' '+ registerViewModel.LName,
                    Email = registerViewModel.Email,
                    PasswordHash = registerViewModel.Password,
                };

                // Save Data base
                IdentityResult result = await userManager.CreateAsync(appUser, registerViewModel.Password);
                if (result.Succeeded)
                {

                    await userManager.AddToRoleAsync(appUser, registerViewModel.Role);
                    
                    // copy data from identity user to emp
                    Employee NewEmployee = new Employee
                    {
                        FName = registerViewModel.FName,
                        LName = registerViewModel.LName,
                        Email = registerViewModel.Email,
                        Phone = registerViewModel.Phone,
                        Role = registerViewModel.Role,
                        CreatedDate = DateTime.Now
                    };
                    
                    employeeRepository.Add(NewEmployee);
                    employeeRepository.Save();


                    // Create Cookie
                    await signInManager.SignInAsync(appUser, false);

                    var currentUser = await userManager.GetUserAsync(User);
                    if (currentUser != null)
                    {
                        currentUser.Employee_id = employeeRepository.GetLastCreatedEmp();
                    }
                    result = await userManager.UpdateAsync(currentUser);


                    return RedirectToAction("Index", "Employee");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            List<IdentityRole>? roles = await roleManager.Roles.ToListAsync();

            registerViewModel.Roles = roles.Select(role => new SelectListItem
            {
                Value = role.Name,
                Text = role.Name
            }).ToList();
            return View("Register", registerViewModel);
        }

        /**************************** Edit ****************************/
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the employee data by ID from the repository
            var employee = employeeRepository.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            // Fetch roles from the database
            var roles = await roleManager.Roles.ToListAsync();

            // Create the ViewModel and populate the Roles and employee data
            var model = new EditViewModel
            {
                EmployeeId = employee.ID,
                FName = employee.FName,
                LName = employee.LName,
                Phone = employee.Phone,
                Email = employee.Email,
                Role = employee.Role,
                Roles = roles.Select(role => new SelectListItem
                {
                    Value = role.Name,
                    Text = role.Name
                }).ToList()
            };

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditViewModel editViewModel)
        {
            if (ModelState.IsValid)
            {
                // Fetch the existing employee and application user
                var employee = employeeRepository.GetById(editViewModel.EmployeeId);
                var appUser = await userManager.Users.FirstOrDefaultAsync(u => u.Employee_id == employee.ID );

                if (employee == null || appUser == null)
                {
                    return NotFound();
                }

                // Update employee data
                employee.FName = editViewModel.FName;
                employee.LName = editViewModel.LName;
                employee.Phone = editViewModel.Phone;
                employee.Email = editViewModel.Email;
                employee.Role = editViewModel.Role;
                employeeRepository.Update(employee);
                employeeRepository.Save();

                // Update user data
                appUser.UserName = editViewModel.FName + editViewModel.LName;
                appUser.Email = editViewModel.Email;

                // Check if role needs to be updated
                var currentRoles = await userManager.GetRolesAsync(appUser);
                if (!currentRoles.Contains(editViewModel.Role))
                {
                    await userManager.RemoveFromRolesAsync(appUser, currentRoles);
                    await userManager.AddToRoleAsync(appUser, editViewModel.Role);
                }

                var result = await userManager.UpdateAsync(appUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Employee");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // Repopulate roles for the view model
            var roles = await roleManager.Roles.ToListAsync();
            editViewModel.Roles = roles.Select(role => new SelectListItem
            {
                Value = role.Name,
                Text = role.Name
            }).ToList();

            return View("Edit", editViewModel);
        }

        /**************************** Log in ****************************/
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

        /**************************** Verify Email ****************************/
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Something is wrong!");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, model.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. try again.");
                return View(model);
            }
        }

        /**************************** Sign Out ****************************/
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return View("Login");
        }

    }
}
