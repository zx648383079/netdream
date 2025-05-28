using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class DeliveryEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int OrderId { get; set; }
        public int Status { get; set; }
        
        public int ShippingId { get; set; }
        
        public string ShippingName { get; set; } = string.Empty;
        
        public float ShippingFee { get; set; }
        
        public string LogisticsNumber { get; set; } = string.Empty;
        
        public string LogisticsContent { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        
        public int RegionId { get; set; }
        
        public string RegionName { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        
        public string BestTime { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
