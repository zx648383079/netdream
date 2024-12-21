
namespace NetDream.Modules.Auth.Entities
{
    
    public class InviteCodeEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
        public int Amount { get; set; }
        
        public int InviteCount { get; set; }
        
        public int ExpiredAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
