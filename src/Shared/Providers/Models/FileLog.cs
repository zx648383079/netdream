using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Models
{

    public class FileOperationResult
    {

        public string Title { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public long Size { get; set; }

        public FileOperationResult()
        {
            
        }

        public FileOperationResult(FileEntity item)
        {
            Title = item.Name;
            Extension = "." + item.Extension;
            Size = item.Size;
            Url = item.Path;
        }
    }
}
