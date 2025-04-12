using Ecom.Core.DTOs;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Ecom.infrastructure.Repositories.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmail(EmailDTO emailDTO)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("My Ecom", configuration["EmailSetting:From"]));
            message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content,
            };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(
                        configuration["EmailSetting:smtp"],
                        int.Parse(configuration["EmailSetting:Port"]), true);

                    await smtp.AuthenticateAsync(configuration["EmailSetting:UserName"]
                        , configuration["EmailSetting:Password"]);

                    await smtp.SendAsync(message);

                }
                catch (Exception)
                {

                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }

        }
    }
}
