
namespace NetDream.Modules.OnlineDisk.Entities
{
    
    public class ServerEntity
    {
        
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        
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
