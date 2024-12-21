
namespace NetDream.Modules.Auth.Entities
{
    
    public class AccountLogEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public byte Type { get; set; }
        
        public int ItemId { get; set; }
        public int Money { get; set; }
        
        public int TotalMoney { get; set; }
        public byte Status { get; set; }
        public string Remark { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
