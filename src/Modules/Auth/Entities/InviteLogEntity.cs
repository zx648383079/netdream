
namespace NetDream.Modules.Auth.Entities
{
    
    public class InviteLogEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int ParentId { get; set; }
        public string Code { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
