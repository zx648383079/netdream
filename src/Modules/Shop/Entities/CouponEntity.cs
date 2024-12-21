
namespace NetDream.Modules.Shop.Entities
{
    
    public class CouponEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public int Type { get; set; }
        public int Rule { get; set; }
        
        public string RuleValue { get; set; } = string.Empty;
        
        public decimal MinMoney { get; set; }
        public decimal Money { get; set; }
        
        public int SendType { get; set; }
        
        public int SendValue { get; set; }
        
        public int EveryAmount { get; set; }
        
        public int StartAt { get; set; }
        
        public int EndAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
