
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class WarehouseLogEntity : IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        
        public int WarehouseId { get; set; }
        
        public int UserId { get; set; }
        
        public int GoodsId { get; set; }
        
        public int ProductId { get; set; }
        public int Amount { get; set; }
        
        public int OrderId { get; set; }
        public string Remark { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
