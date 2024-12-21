
namespace NetDream.Modules.OpenPlatform.Entities
{
    
    public class UserTokenEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int PlatformId { get; set; }
        public string Token { get; set; } = string.Empty;
        
        public int IsSelf { get; set; }
        
        public int ExpiredAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
