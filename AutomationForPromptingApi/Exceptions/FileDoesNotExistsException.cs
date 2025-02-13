namespace AutomationForPromptingApi.Exceptions
{
    public class FileDoesNotExistsException : CustomException
    {
        public FileDoesNotExistsException() : base("File does not exists!") { }
    }
}
