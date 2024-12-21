
namespace NetDream.Modules.Auth.Entities
{
    
    public class LoginLogEntity
    {
        
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public string User { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Mode { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
        
        public int PlatformId { get; set; }
    }
}
