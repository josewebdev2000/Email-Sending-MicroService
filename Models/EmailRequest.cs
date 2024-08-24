namespace EmailSendingMicroService.Models
{
    public class EmailRequest
    {
        public String apiKey { get; set; }
        public String toEmail { get; set; }
        public String subject { get; set; }
        public String? htmlContent { get; set; }
        public String? plainTextContent { get; set; }
    }
}
