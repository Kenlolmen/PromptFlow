namespace AutomationForPromptingApi.Models
{
    public class FileModel
    {
        public string Path { get; set; }

        public FileModel(string path)
        {
            Path = path;
        }
    }
}
