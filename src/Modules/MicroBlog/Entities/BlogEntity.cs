
namespace NetDream.Modules.MicroBlog.Entities
{
    
    public class BlogEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public string ExtraRule { get; set; } = string.Empty;
        
        public byte OpenType { get; set; }
        
        public int RecommendCount { get; set; }
        
        public int CollectCount { get; set; }
        
        public int ForwardCount { get; set; }
        
        public int CommentCount { get; set; }
        
        public int ForwardId { get; set; }
        public string Source { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
