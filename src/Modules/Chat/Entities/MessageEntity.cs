
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Entities
{
    
    public class MessageEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public int Type { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public string ExtraRule { get; set; } = string.Empty;
        
        public int ItemId { get; set; }
        
        public int ReceiveId { get; set; }
        
        public int GroupId { get; set; }
        
        public int UserId { get; set; }
        public int Status { get; set; }
        
        public int DeletedAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
