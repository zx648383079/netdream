
namespace NetDream.Modules.Shop.Entities
{
    
    public class ArticleEntity
    {
        
        public int Id { get; set; }
        
        public int CatId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
