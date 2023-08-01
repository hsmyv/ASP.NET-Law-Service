using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Services.Interfaces;
using Aspİntro.Utilities.Helpers;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Aspİntro.Services
{
    public class EmailService : IEmailService
    {  
        private readonly IConfiguration _config;

            public EmailService (IConfiguration config)
            {
                _config = config;
            }

        public async Task SendEmailAsync(string emailTo, string userName, string url, string html, string content)
        {
            var emailModel = _config.GetSection("EmailConfig").Get<EmailRequest>();
            var apiKey = emailModel.SecretKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(emailModel.SenderEmail, emailModel.SenderName);
            var subject = "Verification Email";
            var to = new EmailAddress(emailTo, userName);
            var plainTextContent = content;
            var htmlContent = html;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        //$"<a href={url} >Click here</a>
    }
}
