namespace EmailSendingMicroService.Views
{
    public class EmailSentView
    {
        public static Dictionary<string, object> EmailSentResponse(String message, bool successStatus)
        {
            return new Dictionary<string, object> {
                { "message", message },
                { "success", successStatus }
            };
        }
    }
}
