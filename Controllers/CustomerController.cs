using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    public class CustomerController : Controller
    {
        private readonly INorthwindRepository repository;
        private readonly UserManager<AppUser> userManager;
        public CustomerController(INorthwindRepository repository, UserManager<AppUser> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CustomerWithPassword customerWithPassword)
        {
            if (!ModelState.IsValid) return View();
            var customer = customerWithPassword.Customer;
            if (repository.Customers.Any(c => c.CompanyName == customer.CompanyName))
            {
                ModelState.AddModelError("", "The company name must be unique!");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var user = new AppUser
                    {
                        Email = customer.Email,
                        UserName = customer.Email
                    };

                    var result = await userManager.CreateAsync(user, customerWithPassword.Password);
                    if (!result.Succeeded)
                    {
                        AddErrorsFromResult(result);
                    }
                    else
                    {
                        result = await userManager.AddToRoleAsync(user, "Customer");

                        if (!result.Succeeded)
                        {
                            await userManager.DeleteAsync(user);
                            AddErrorsFromResult(result);
                        }
                        else
                        {
                            repository.AddCustomer(customer);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            return View();
        }

        [Authorize(Roles = "Customer")]
        public IActionResult Account() => View(repository.Customers.FirstOrDefault(c => c.Email == User.Identity.Name));

        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Account(Customer customer)
        {
            repository.EditCustomer(customer);
            return RedirectToAction("Index", "Home");
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