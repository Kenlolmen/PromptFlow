namespace AutomationForPromptingApi.Exceptions
{
    public class OpenAiFailedResponseException : CustomException
    {
        public OpenAiFailedResponseException() : base("Failed to get response from OpenAI.")
        {
        }
    }
}
