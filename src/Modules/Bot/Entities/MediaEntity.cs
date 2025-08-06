using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Bot.Entities
{
    
    public class MediaEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int BotId { get; set; }
        public string Type { get; set; } = string.Empty;
        
        public byte MaterialType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        
        public byte ShowCover { get; set; }
        
        public byte OpenComment { get; set; }
        
        public byte OnlyComment { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        
        public string MediaId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        
        public byte PublishStatus { get; set; }
        
        public string PublishId { get; set; } = string.Empty;
        
        public int ExpiredAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
