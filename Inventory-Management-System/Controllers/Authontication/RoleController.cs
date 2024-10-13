﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.ViewModel
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public IActionResult AddRole()
        {
            return View("Add");
        }

        [HttpPost]
        public async Task<IActionResult> SaveRole(RoleViewModel RoleViewModel)
        {
            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole();
                identityRole.Name = RoleViewModel.RoleName;
               IdentityResult result = await roleManager.CreateAsync(identityRole);
                if(result.Succeeded)
                {
                    ViewBag.sucess = true;  
                    return View("Add");
                }
                foreach(var error in  result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("Add", RoleViewModel);
        }
    }
}
