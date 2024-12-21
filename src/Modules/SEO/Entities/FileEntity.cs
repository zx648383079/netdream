
namespace NetDream.Modules.SEO.Entities
{
    
    public class FileEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Md5 { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int Folder { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
