using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Storage.Models
{
    public class FileListItem(string name) : IFileListItem
    {

        public string Name => name;

        public string Type { get; set; }

        public long Size { get; set; }

        public string Thumb { get; set; }

        public string Url { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
      
    }
}
