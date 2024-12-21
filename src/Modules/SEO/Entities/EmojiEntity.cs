
namespace NetDream.Modules.SEO.Entities
{
    
    public class EmojiEntity
    {
        
        public int Id { get; set; }
        
        public int CatId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
