using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class GoodsAttributeEntity : IIdEntity
    {
        
        public int Id { get; set; }
        
        public int GoodsId { get; set; }
        
        public int AttributeId { get; set; }
        public string Value { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
