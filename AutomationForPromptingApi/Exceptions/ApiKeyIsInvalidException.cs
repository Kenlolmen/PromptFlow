namespace AutomationForPromptingApi.Exceptions
{
    public class ApiKeyIsInvalidException : CustomException
    {
        public ApiKeyIsInvalidException() : base("The API key is invalid.")
        {
        }
    }
}
