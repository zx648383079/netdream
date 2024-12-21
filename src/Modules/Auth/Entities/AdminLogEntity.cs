
namespace NetDream.Modules.Auth.Entities
{
    
    public class AdminLogEntity
    {
        
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public int ItemType { get; set; }
        
        public int ItemId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
