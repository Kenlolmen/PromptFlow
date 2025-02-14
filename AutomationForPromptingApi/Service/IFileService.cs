using AutomationForPromptingApi.Models;

namespace AutomationForPromptingApi.Service
{
    public interface IFileService
    {
        List<Dictionary<string, string>> ReadFile(FileModel fileModel);
        void SaveFile(FileModel fileModel, List<Dictionary<string, string>> data);
        public List<string> LoadKeywords(FileModel fileModel);
    }
}
