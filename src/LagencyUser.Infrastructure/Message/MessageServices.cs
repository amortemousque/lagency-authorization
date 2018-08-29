using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationEvents;
using LagencyUser.Application.Events;
using LagencyUser.Web.Services;
using Rebus.Bus;

namespace LagencyUser.Infrastructure.Message
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly IBus _bus;  

        public AuthMessageSender(IBus bus) 
        {
            _bus = bus;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var test = "";
            // Plug in your email service here to send an email.
            await _bus.Send(new SendEmail(new List<EmailRecipient> { new EmailRecipient{ Address = email } }, subject, message, message));
            //await _bus.Advanced.Routing.Send("notification.sendMail", new SendEmail(email, subject, message));
            //return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
