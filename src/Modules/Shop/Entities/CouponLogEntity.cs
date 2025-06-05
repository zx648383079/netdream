using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class CouponLogEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int CouponId { get; set; }
        
        public string SerialNumber { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public int OrderId { get; set; }
        
        public int UsedAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
