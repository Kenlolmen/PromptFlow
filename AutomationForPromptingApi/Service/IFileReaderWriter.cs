using AutomationForPromptingApi.Models;

namespace AutomationForPromptingApi.Service
{
    public interface IFileReaderWriter
    {
        bool CheckFileExists(FileModel fileModel);

        List<Dictionary<string, string>> ReadFile(FileModel fileModel);
        void SaveFile(FileModel fileModel, List<Dictionary<string, string>> data);
    }
}
