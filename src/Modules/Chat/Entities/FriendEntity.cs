using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Entities
{
    
    public class FriendEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int ClassifyId { get; set; }
        
        public int UserId { get; set; }
        
        public int BelongId { get; set; }
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
