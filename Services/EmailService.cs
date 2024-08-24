using MimeKit;
using MailKit.Net.Smtp;

namespace EmailSendingMicroService.Services
{
    public class EmailService
    {
        // Set up constants for 
        private readonly String smtpHost;
        private readonly String smtpUsername;
        private readonly String smtpEmail;
        private readonly String smtpPass;
        private readonly int smtpPort;

        // Set up IConfiguration for the Web API
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            // Get SMTP Required Info to send Emails
            smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST");
            smtpUsername = Environment.GetEnvironmentVariable("SMTP_USERNAME");
            smtpEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL");
            smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS");
            smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT"));

            // Initialize Configuration for Web API
            _configuration = configuration;
        }

        private async Task sendPreparedEmail(MimeMessage emailMessage)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    await smtpClient.ConnectAsync(smtpHost, smtpPort, false);
                    await smtpClient.AuthenticateAsync(smtpEmail, smtpPass);
                    await smtpClient.SendAsync(emailMessage);
                    await smtpClient.DisconnectAsync(true);
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Email Could Not Be Sent", ex);
            }
        }

        public async Task sendPlainTextEmailAsync(string toEmail, string subject, string plainTextContent)
        {
            MimeMessage emailMessage = new MimeMessage();

            // Add Info about the Sender
            emailMessage.From.Add(
                new MailboxAddress(
                    smtpUsername,
                    smtpEmail
                )    
            );

            // Add Info about the Recipient
            emailMessage.To.Add(
                new MailboxAddress(
                    "",
                    toEmail
                )
            );

            // Add Info About The Subject
            emailMessage.Subject = subject;

            // Add the plain text content
            emailMessage.Body = new TextPart("plain")
            {
                Text = plainTextContent
            };

            // Send the email
            await sendPreparedEmail(emailMessage);
        }

        public async Task sendHtmlAsEmailAsync(string toEmail, string subject, string htmlContent)
        {
            // Prepare the email message to send
            MimeMessage emailMessage = new MimeMessage();

            // Add Info about the Sender 
            emailMessage.From.Add(
                new MailboxAddress(
                        smtpUsername,
                        smtpEmail
                    )
            );

            // Add Info about the Recipient
            emailMessage.To.Add(
                new MailboxAddress(
                    "",
                    toEmail)
            );

            // Add Info about the Subject
            emailMessage.Subject = subject;

            // Add the HTML Template content
            emailMessage.Body = new TextPart("html")
            {
                Text = htmlContent
            };

            // Send the email
            await sendPreparedEmail(emailMessage);
        }
    }
}
