
namespace NetDream.Modules.Shop.Entities
{
    
    public class GoodsEntity
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
        
        public decimal MarketPrice { get; set; }
        public int Stock { get; set; }
        
        public int AttributeGroupId { get; set; }
        public float Weight { get; set; }
        
        public int ShippingId { get; set; }
        public int Sales { get; set; }
        
        public int IsBest { get; set; }
        
        public int IsHot { get; set; }
        
        public int IsNew { get; set; }
        public int Status { get; set; }
        
        public string AdminNote { get; set; } = string.Empty;
        public int Type { get; set; }
        public int Position { get; set; }
        
        public int DynamicPosition { get; set; }
        
        public int DeletedAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
        
        public decimal CostPrice { get; set; }
    }
}
