using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;

namespace Northwind.Controllers
{
    public class RecoverController : Controller
    {
        private readonly INorthwindRepository _repository;
        private readonly UserManager<AppUser> _userManager;

        public RecoverController(INorthwindRepository repo, UserManager<AppUser> usrMgr)
        {
            _repository = repo;
            _userManager = usrMgr;
        }

        public IActionResult Main()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Sent(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (_repository.Customers.Any(c => c.Email == customer.Email))
                {
                    //Add in code to generate token and send email
                }
            }

            return View();
        }



    }
}
