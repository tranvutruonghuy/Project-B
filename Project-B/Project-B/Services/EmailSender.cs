
namespace Project_B.Services
{

    using Microsoft.AspNetCore.Identity.UI.Services;
    using MailKit.Net.Smtp;
    using MimeKit;

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Online medicine store", _configuration["EmailSettings:From"]));
            emailMessage.To.Add(MailboxAddress.Parse(email)); // Sử dụng phương thức Parse để phân giải địa chỉ email
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = htmlMessage };

            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

    }


}
