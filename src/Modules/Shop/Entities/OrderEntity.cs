
namespace NetDream.Modules.Shop.Entities
{
    
    public class OrderEntity
    {
        
        public int Id { get; set; }
        
        public string SeriesNumber { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public int Status { get; set; }
        
        public int PaymentId { get; set; }
        
        public string PaymentName { get; set; } = string.Empty;
        
        public int ShippingId { get; set; }
        
        public int InvoiceId { get; set; }
        
        public string ShippingName { get; set; } = string.Empty;
        
        public decimal GoodsAmount { get; set; }
        
        public decimal OrderAmount { get; set; }
        public decimal Discount { get; set; }
        
        public decimal ShippingFee { get; set; }
        
        public decimal PayFee { get; set; }
        
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
