using MailKit.Net.Smtp;
using MimeKit;
using HRMS.Notification;
using System.Threading.Tasks;
using HRMS.Interfaces;

namespace HRMS.Services
{
    public class AuthMessageSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

       public AuthMessageSender(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            EmailMessage emailMessage = new EmailMessage();
            emailMessage.Sender = new MailboxAddress(_emailSettings.Sender, _emailSettings.Sender);
            emailMessage.Reciever = new MailboxAddress(email, email);
            emailMessage.Subject = subject;
            emailMessage.Content = message;
            var mimeMessage = CreateMimeMessageFromEmailMessage(emailMessage, MimeKit.Text.TextFormat.Html);

            using (SmtpClient smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_emailSettings.SmtpServer,
                _emailSettings.Port, true);
                await smtpClient.AuthenticateAsync(_emailSettings.UserName,
                _emailSettings.Password);
                await smtpClient.SendAsync(mimeMessage);
                await smtpClient.DisconnectAsync(true);
            }
        }

        private MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message, MimeKit.Text.TextFormat textFormat)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Reciever);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(textFormat)
            { Text = message.Content };
            return mimeMessage;
        }
    }
}
