namespace AutomationForPromptingApi.Exceptions
{
    public class EmptyFileException : CustomException
    {
        public EmptyFileException() : base("File is empty!") { }

    }
}
