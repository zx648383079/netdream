
namespace NetDream.Modules.Shop.Entities
{
    
    public class GoodsMetaEntity
    {
        
        public int Id { get; set; }
        
        public int GoodsId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
