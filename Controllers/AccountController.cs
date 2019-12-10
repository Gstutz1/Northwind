using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Northwind.Services;

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

            if (user == null) return View(login);
            await signInManager.SignOutAsync();

            var result = 
                await signInManager.PasswordSignInAsync(user, login.Password,
                    false, false);

            if (result.Succeeded)
            {
                return Redirect(returnUrl ?? "/");
            }
            ModelState.AddModelError(nameof(LoginModel.Email), "Invalid user of password");
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

        [HttpPost]
        public async Task<IActionResult> PasswordReset(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var code = await userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Action("ChangePassword", "Account", new { UserId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    var emailManager = new EmailService();
                    await emailManager.SendEmailAsync(email, "Password Reset Link", "Please reset your password by clicking here: " + callbackUrl);
                }
            }

            return View();
        }

        public IActionResult ChangePassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordViewModel reset)
        {
            var user = await userManager.FindByEmailAsync(reset.Email);

            if (user != null)
            {
                var result = userManager.ResetPasswordAsync(user, reset.Token, reset.Password).Result;

                if (result.Succeeded)
                {
                    return View("ResetSuccess");
                }
            }

            return View(reset);
        }
    }
}
