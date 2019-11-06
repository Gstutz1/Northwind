using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Northwind.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel login, string returnUrl)
        {
            if (!ModelState.IsValid) return View(login);

            var user = await userManager.FindByEmailAsync(login.Email);

            if (user != null)
            {
                await signInManager.SignOutAsync();

                var result = 
                    await signInManager.PasswordSignInAsync(user, login.Password,
                        false, false);

                if (result.Succeeded)
                {
                    return Redirect(returnUrl ?? "/");
                }
                ModelState.AddModelError(nameof(LoginModel.Email), "Invalid user of password");
            }
            return View(login);
        }

        [AllowAnonymous]
        public ViewResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
