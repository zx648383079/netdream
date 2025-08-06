using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Bot.Entities
{
    
    public class ReplyEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int BotId { get; set; }
        public string Event { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public byte Match { get; set; }
        public string Content { get; set; } = string.Empty;
        public byte Type { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
        public byte Status { get; set; }
    }
}
