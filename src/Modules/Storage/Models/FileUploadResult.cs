namespace NetDream.Modules.Storage.Models
{
    public class FileUploadResult(string name): FileListItem(name)
    {
        public string Original { get; set; }
    }
}
