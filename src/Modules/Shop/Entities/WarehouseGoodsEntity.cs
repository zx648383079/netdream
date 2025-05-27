
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class WarehouseGoodsEntity : IIdEntity
    {
        
        public int Id { get; set; }
        
        public int WarehouseId { get; set; }
        
        public int GoodsId { get; set; }
        
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
