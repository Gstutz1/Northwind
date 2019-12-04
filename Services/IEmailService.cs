using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Services
{
    interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
