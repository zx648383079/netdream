
namespace NetDream.Modules.Auth.Entities
{
    
    public class BanAccountEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public string ItemKey { get; set; } = string.Empty;
        
        public int ItemType { get; set; }
        
        public int PlatformId { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
