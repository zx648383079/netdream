
namespace NetDream.Modules.Shop.Entities
{
    
    public class WarehouseGoodsEntity
    {
        
        public int Id { get; set; }
        
        public int WarehouseId { get; set; }
        
        public int GoodsId { get; set; }
        
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
