using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Northwind.Services
{
    public class EmailService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                // Message sent to user
                var message = new MimeMessage();

                // User email
                message.To.Add(new MailboxAddress("blank", email));

                // Our email
                message.From.Add(new MailboxAddress("blank", "blank@blank.com"));

                // Subject and body of email
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = htmlMessage
                };

                using (var client = new SmtpClient())
                {
                    // Connects to gmail server
                    client.Connect("smtp.gmail.com", 587, false);

                    // Login to gmail server
                    client.Authenticate("myname@company.com", "password");

                    // Send message to user email
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
