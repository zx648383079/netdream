using NetDream.Shared.Interfaces.Entities;

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
