using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    public class CustomerController : Controller
    {
        private INorthwindRepository repository;
        public CustomerController(INorthwindRepository repo) => repository = repo;

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (repository.Customers.Any(c => c.CompanyName == customer.CompanyName))
                {
                    ModelState.AddModelError("", "The company name must be unique!");
                }
                else
                {
                    repository.AddCustomer(customer);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        public IActionResult Account() => View(repository.Customers.FirstOrDefault(c => c.Email == User.Identity.Name));

        public IActionResult Account(Customer customer)
        {
            repository.EditCustomer(customer);
            return RedirectToAction("Index", "Home");
        }
    }
}