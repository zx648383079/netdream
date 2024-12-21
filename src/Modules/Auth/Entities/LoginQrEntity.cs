
namespace NetDream.Modules.Auth.Entities
{
    
    public class LoginQrEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int ExpiredAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
        
        public int PlatformId { get; set; }
    }
}
