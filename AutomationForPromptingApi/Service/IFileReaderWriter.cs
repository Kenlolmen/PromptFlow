using AutomationForPromptingApi.Models;

namespace AutomationForPromptingApi.Service
{
    public interface IFileReaderWriter
    {
        bool CheckFileExists(FileModel fileModel);
    }
}
