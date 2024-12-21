
namespace NetDream.Modules.Auth.Entities
{
    
    public class OauthEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int PlatformId { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string Vendor { get; set; } = string.Empty;
        public string Unionid { get; set; } = string.Empty;
        public string Identity { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
