using System;
using System.Threading.Tasks;

namespace MyIdentity.Email
{
    public interface IEmailSender
    {
        Task<SendEmailResponse> SendEmailAsync(string userEmail, string emailSubject, string message);
    }
}
