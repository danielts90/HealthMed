using HealthMed.Shared.Dtos;
using System.Net;
using System.Net.Mail;

namespace HealthMed.Shared.Util
{
    public interface IEmailService
    {
        Task<bool> SendMail(EmailDto email);
    }

    public class EmailService : IEmailService
    {
        public EmailService()
        {
        }

        public async Task<bool> SendMail(EmailDto email)
        {
            var client = new SmtpClient("live.smtp.mailtrap.io", 587)
            {
                Credentials = new NetworkCredential("api", "764d787424c91c821fd82a080718bdfe"),
                EnableSsl = true
            };

            //Fixing the parameters to show in the presentation.
            var mailMessage = new MailMessage("hello@demomailtrap.com", "rm356034@fiap.com.br", email.Subject, email.Body);
            mailMessage.IsBodyHtml = true;

            await client.SendMailAsync(mailMessage);

            return true;
        }

    }
}
