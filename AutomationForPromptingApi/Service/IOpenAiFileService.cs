namespace AutomationForPromptingApi.Service
{
    public interface IOpenAiFileService
    {
        Task<string> SendToChatGptAsync(List<string> keywords);
    }
}
