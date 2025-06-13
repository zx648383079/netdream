using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class OrderRefundEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int OrderId { get; set; }
        
        public int OrderGoodsId { get; set; }
        
        public int GoodsId { get; set; }
        
        public int ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Amount { get; set; }
        public byte Type { get; set; }
        public byte Status { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Evidence { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public decimal Money { get; set; }
        
        public decimal OrderPrice { get; set; }
        public int Freight { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
