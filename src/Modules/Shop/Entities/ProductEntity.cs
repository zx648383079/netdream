
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class ProductEntity: IIdEntity
    {
        
        public int Id { get; set; }
        
        public int GoodsId { get; set; }
        public float Price { get; set; }
        
        public float MarketPrice { get; set; }
        public int Stock { get; set; }
        public float Weight { get; set; }
        
        public string SeriesNumber { get; set; } = string.Empty;
        public string Attributes { get; set; } = string.Empty;
    }
}
