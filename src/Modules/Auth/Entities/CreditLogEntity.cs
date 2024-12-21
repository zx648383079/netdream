
namespace NetDream.Modules.Auth.Entities
{
    
    public class CreditLogEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public int Type { get; set; }
        
        public int ItemId { get; set; }
        public int Credits { get; set; }
        
        public int TotalCredits { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
