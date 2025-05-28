using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class OrderAddressEntity : IIdEntity
    {
        
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int RegionId { get; set; }
        
        public string RegionName { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        
        public string BestTime { get; set; } = string.Empty;
    }
}
