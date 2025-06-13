using NetDream.Modules.Shop.Market;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class GoodsEntity : IIdEntity, ITimestampEntity, IProductSource
    {
        
        public int Id { get; set; }
        
        public int CatId { get; set; }
        
        public int BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public string SeriesNumber { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public decimal MarketPrice { get; set; }
        public int Stock { get; set; }
        
        public int AttributeGroupId { get; set; }
        public decimal Weight { get; set; }
        
        public int ShippingId { get; set; }
        public byte Sales { get; set; }
        
        public byte IsBest { get; set; }
        
        public byte IsHot { get; set; }
        
        public byte IsNew { get; set; }
        public byte Status { get; set; }
        
        public string AdminNote { get; set; } = string.Empty;
        public byte Type { get; set; }
        public int Position { get; set; }
        
        public int DynamicPosition { get; set; }
        
        public int DeletedAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
        

    }
}
