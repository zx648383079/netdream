
namespace NetDream.Modules.Blog.Entities
{
    
    public class ClickLogEntity
    {
        
        public int Id { get; set; }
        
        public string HappenDay { get; set; } = string.Empty;
        
        public int BlogId { get; set; }
        
        public int ClickCount { get; set; }
    }
}
