using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Bot.Entities
{
    
    public class QrcodeEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int BotId { get; set; }

        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }
        
        public byte SceneType { get; set; }
        
        public string SceneStr { get; set; } = string.Empty;
        
        public int SceneId { get; set; }
        
        public int ExpireTime { get; set; }
        
        public string QrUrl { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }

    }
}
