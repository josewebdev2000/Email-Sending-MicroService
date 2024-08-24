using EmailSendingMicroService.Models;
using EmailSendingMicroService.Services;
using EmailSendingMicroService.Views;
using Microsoft.AspNetCore.Mvc;

namespace EmailSendingMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendController : Controller
    {
        // Define the required API key
        private readonly String apiKey;

        // Set up the Email Service
        private readonly EmailService emailService;

        public SendController(EmailService emailService)
        {
            this.emailService = emailService;
            this.apiKey = Environment.GetEnvironmentVariable("API_KEY");
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null || string.IsNullOrEmpty(emailRequest.toEmail))
            {
                return BadRequest(EmailSentView.EmailSentResponse("Invalid email request", false));
            }

            // If the API KEY Is not in the Request, send a BadRequest
            if (string.IsNullOrEmpty(emailRequest.apiKey) || !emailRequest.apiKey.Equals(this.apiKey))
            {
                return BadRequest(EmailSentView.EmailSentResponse("Invalid API Key",false));
            }

            try
            {
                // Check if HTML Template will be sent
                if (!string.IsNullOrEmpty(emailRequest.htmlContent))
                {
                    await emailService.sendHtmlAsEmailAsync(emailRequest.toEmail, emailRequest.subject, emailRequest.htmlContent);
                }

                // Check if plain text will be sent
                else if (!string.IsNullOrEmpty(emailRequest.plainTextContent))
                {
                    await emailService.sendPlainTextEmailAsync(emailRequest.toEmail, emailRequest.subject, emailRequest.plainTextContent);
                }

                // Otherwise, return a Bad Request for lack of content
                else
                {
                    return BadRequest(EmailSentView.EmailSentResponse("No content provided for the email", false));
                }

                return Ok(EmailSentView.EmailSentResponse($"Email was sent successfully to {emailRequest.toEmail}", true));
            }

            catch (Exception ex) 
            {
                return StatusCode(500, EmailSentView.EmailSentResponse($"Internal server error: {ex.Message}", false));
            }
        }

    }
}
