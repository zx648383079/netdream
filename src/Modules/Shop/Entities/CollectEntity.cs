using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Shop.Entities
{
    
    public class CollectEntity : IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int GoodsId { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
