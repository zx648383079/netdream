
namespace NetDream.Modules.Shop.Entities
{
    
    public class AffiliateLogEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int ItemType { get; set; }
        
        public int ItemId { get; set; }
        
        public string OrderSn { get; set; } = string.Empty;
        
        public decimal OrderAmount { get; set; }
        public decimal Money { get; set; }
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
