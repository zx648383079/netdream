namespace NetDream.Modules.Storage.Models
{
    public class VirtualFileItem(string name, string path): FileListItem(name)
    {
        public string Path { get; set; } = path;

        public bool IsFolder { get; set; }

        public VirtualFileItem(string name, string path, bool isFolder): this(name, path)
        {
            IsFolder = isFolder;
        }
    }
}
