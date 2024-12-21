
namespace NetDream.Modules.Blog.Entities
{
    
    public class BlogMetaEntity
    {
        
        public int Id { get; set; }
        
        public int BlogId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
