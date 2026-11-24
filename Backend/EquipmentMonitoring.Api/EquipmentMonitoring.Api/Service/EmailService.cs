using EquipmentMonitoring.Api.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace EquipmentMonitoring.Api.Services
{
    public class EmailService
    {
        private readonly SmtpSettings _settings;

        public EmailService(
            IOptions<SmtpSettings> options
        )
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(
            string recipient,
            string subject,
            string body
        )
        {
            using var client =
                new SmtpClient(
                    _settings.Host,
                    _settings.Port
                );

            client.Credentials =
                new NetworkCredential(
                    _settings.Username,
                    _settings.Password
                );

            client.EnableSsl =
                _settings.EnableSsl;

            var message =
                new MailMessage(
                    _settings.FromEmail,
                    recipient,
                    subject,
                    body
                );
            message.IsBodyHtml = true;

            await client
                .SendMailAsync(
                    message
                );
        }
    }
}