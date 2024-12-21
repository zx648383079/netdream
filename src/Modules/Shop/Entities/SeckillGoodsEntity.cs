
namespace NetDream.Modules.Shop.Entities
{
    
    public class SeckillGoodsEntity
    {
        
        public int Id { get; set; }
        
        public int ActId { get; set; }
        
        public int TimeId { get; set; }
        
        public int GoodsId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        
        public int EveryAmount { get; set; }
    }
}
