
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class ProductEntity: IIdEntity
    {
        
        public int Id { get; set; }
        
        public int GoodsId { get; set; }
        public decimal Price { get; set; }
        
        public decimal MarketPrice { get; set; }
        public int Stock { get; set; }
        public decimal Weight { get; set; }
        
        public string SeriesNumber { get; set; } = string.Empty;
        public string Attributes { get; set; } = string.Empty;
    }
}
