namespace NetDream.Shared.Repositories.Models
{
    public class FileItem(string name)
    {
        public string Name { get; set; } = name;

        public long Size { get; set; }

        public int CreatedAt { get; set; }
    }
}
