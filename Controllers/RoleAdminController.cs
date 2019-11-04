using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    public class RoleAdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RoleAdminController(RoleManager<IdentityRole> roleManager, 
            UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Required]string name)
        {
            if (!ModelState.IsValid) return View(name);

            var result = await roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AddErrorsFromResult(result);
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "No role found");
            }
            return View("Index", roleManager.Roles);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var members = new List<AppUser>();
            var nonMembers = new List<AppUser>();

            foreach (var user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (!ModelState.IsValid) return await Edit(model.RoleId);
            foreach (var userId in model.IdsToAdd ?? new string[] { })
            {
                var user = await userManager.FindByIdAsync(userId);

                if (user == null) continue;

                result = await userManager.AddToRoleAsync(user, model.RoleName);
                if (!result.Succeeded)
                {
                    AddErrorsFromResult(result);
                }
            }
            foreach (var userId in model.IdsToDelete ?? new string[] { })
            {
                var user = await userManager.FindByIdAsync(userId);

                if (user == null) continue;

                result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                if (!result.Succeeded)
                {
                    AddErrorsFromResult(result);
                }
            }
            return await Edit(model.RoleId);
        }


        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
