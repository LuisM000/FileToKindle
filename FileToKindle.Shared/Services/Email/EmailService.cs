using System;
using System.Collections.Generic;
using System.Linq;
using FileToKindle.Shared.Models;
using FileToKindle.Shared.Services;
using MimeKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(EmailService))]
namespace FileToKindle.Shared.Services
{
    public class EmailService : IEmailService
    {
        public void Send(FullEmail origin, Email destination, IList<string> attachments)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(origin.Address));
            message.To.Add(new MailboxAddress(destination.Address));
            if (attachments != null)
            {
                var builder = new BodyBuilder();
                attachments.ToList().ForEach(f => builder.Attachments.Add(f));
                message.Body = builder.ToMessageBody();
            }

            using var client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect(origin.Host, origin.Port, false);
            client.Authenticate(origin.Address, origin.Password);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
