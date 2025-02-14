using AutomationForPromptingApi.Models;
using System.Collections.Generic;

namespace AutomationForPromptingApi.Service
{
    public class FileService
    {
        private readonly IFileService _fileService;

        public FileService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public List<Dictionary<string, string>> ReadData(FileModel fileModel)
        {
            return _fileService.ReadFile(fileModel);
        }

        public void SaveData(FileModel fileModel, List<Dictionary<string, string>> data)
        {
            _fileService.SaveFile(fileModel, data);
        }

        public List<string> GetKeywords(FileModel fileModel)
        {
            return _fileService.LoadKeywords(fileModel);
        }
    }
}
