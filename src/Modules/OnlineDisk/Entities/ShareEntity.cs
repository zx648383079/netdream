
namespace NetDream.Modules.OnlineDisk.Entities
{
    
    public class ShareEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Mode { get; set; }
        public string Password { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public int DeathAt { get; set; }
        
        public int ViewCount { get; set; }
        
        public int DownCount { get; set; }
        
        public int SaveCount { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
