namespace AutomationForPromptingApi.Exceptions
{
    public abstract class CustomException : Exception
    {
        protected CustomException(string messege) : base(messege) { }

    }
}
