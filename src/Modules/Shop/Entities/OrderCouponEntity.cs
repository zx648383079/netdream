
namespace NetDream.Modules.Shop.Entities
{
    
    public class OrderCouponEntity
    {
        
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        
        public int CouponId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
