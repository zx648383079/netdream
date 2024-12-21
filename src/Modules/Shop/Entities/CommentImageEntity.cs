
namespace NetDream.Modules.Shop.Entities
{
    
    public class CommentImageEntity
    {
        
        public int Id { get; set; }
        
        public int CommentId { get; set; }
        public string Image { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
