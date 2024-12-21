
namespace NetDream.Modules.Blog.Entities
{
    
    public class TagEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public int BlogCount { get; set; }
    }
}
