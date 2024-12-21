
namespace NetDream.Modules.Auth.Entities
{
    
    public class ApplyLogEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public int Type { get; set; }
        public int Money { get; set; }
        public string Remark { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
