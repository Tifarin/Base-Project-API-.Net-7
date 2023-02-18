using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BASEAPI.Models.Dtos;
using MailKit.Net.Smtp;
using MimeKit;

namespace BASEAPI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config) {
            _config = config;
        }
        public void SendEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html){Text = request.Body};
            
            using var smtp =  new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value,587,MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUserName").Value,_config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}