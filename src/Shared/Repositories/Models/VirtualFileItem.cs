namespace NetDream.Shared.Repositories.Models
{
    public class VirtualFileItem(string name, string path): FileItem(name)
    {
        public string Path { get; set; } = path;

        public bool IsFolder { get; set; }

        public VirtualFileItem(string name, string path, bool isFolder): this(name, path)
        {
            IsFolder = isFolder;
        }
    }
}
