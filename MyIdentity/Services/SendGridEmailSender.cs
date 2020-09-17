using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MyIdentity.Email;
using MyIdentity.EntityLayer;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyIdentity.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly AppSetting _appSettings;

        public SendGridEmailSender(IOptions<AppSetting> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<SendEmailResponse> SendEmailAsync(string userEmail, string emailSubject, string message)
        {
            var apiKey = _appSettings.SendGridKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("mayanksciphy@gmail.com", "mayank");
            var subject = emailSubject;
            var to = new EmailAddress(userEmail, "mayank");
            var plainTextContent = message;
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return new SendEmailResponse();
        }
    }
}
