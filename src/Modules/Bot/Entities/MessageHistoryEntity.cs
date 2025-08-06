using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Bot.Entities
{
    
    public class MessageHistoryEntity : IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        
        public int BotId { get; set; }
        public byte Type { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
        
        public byte ItemType { get; set; }
        
        public int ItemId { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public byte IsMark { get; set; }
    }
}
