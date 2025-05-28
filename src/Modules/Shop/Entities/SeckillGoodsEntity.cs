using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class SecKillGoodsEntity : IIdEntity
    {
        
        public int Id { get; set; }
        
        public int ActId { get; set; }
        
        public int TimeId { get; set; }
        
        public int GoodsId { get; set; }
        public float Price { get; set; }
        public int Amount { get; set; }
        
        public int EveryAmount { get; set; }
    }
}
