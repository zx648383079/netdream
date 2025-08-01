using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineDisk.Entities
{
    
    public class ServerEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public int Port { get; set; }
        
        public byte CanUpload { get; set; }
        
        public string UploadUrl { get; set; } = string.Empty;
        
        public string DownloadUrl { get; set; } = string.Empty;
        
        public string PingUrl { get; set; } = string.Empty;
        
        public int FileCount { get; set; }
        public byte Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
