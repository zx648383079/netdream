using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class CommentEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int ItemType { get; set; }
        
        public int ItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Rank { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
