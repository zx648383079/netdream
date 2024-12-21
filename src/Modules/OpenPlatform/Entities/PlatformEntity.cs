using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OpenPlatform.Entities
{
    
    public class PlatformEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Domain { get; set; } = string.Empty;
        public string Appid { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        
        public byte SignType { get; set; }
        
        public string SignKey { get; set; } = string.Empty;
        
        public byte EncryptType { get; set; }
        
        public string PublicKey { get; set; } = string.Empty;
        public string Rules { get; set; } = string.Empty;
        public byte Status { get; set; }
        
        public int AllowSelf { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
