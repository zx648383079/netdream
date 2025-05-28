using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class OrderEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public string SeriesNumber { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public byte Status { get; set; }
        
        public int PaymentId { get; set; }
        
        public string PaymentName { get; set; } = string.Empty;
        
        public int ShippingId { get; set; }
        
        public int InvoiceId { get; set; }
        
        public string ShippingName { get; set; } = string.Empty;
        
        public float GoodsAmount { get; set; }
        
        public float OrderAmount { get; set; }
        public float Discount { get; set; }
        
        public float ShippingFee { get; set; }
        
        public float PayFee { get; set; }
        
        public int PayAt { get; set; }
        
        public int ShippingAt { get; set; }
        
        public int ReceiveAt { get; set; }
        
        public int FinishAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
        
        public int ReferenceType { get; set; }
        
        public int ReferenceId { get; set; }
    }
}
