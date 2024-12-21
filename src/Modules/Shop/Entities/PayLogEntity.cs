
namespace NetDream.Modules.Shop.Entities
{
    
    public class PayLogEntity
    {
        
        public int Id { get; set; }
        
        public int PaymentId { get; set; }
        
        public string PaymentName { get; set; } = string.Empty;
        public int Type { get; set; }
        
        public int UserId { get; set; }
        public string Data { get; set; } = string.Empty;
        public int Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        
        public decimal CurrencyMoney { get; set; }
        
        public string TradeNo { get; set; } = string.Empty;
        
        public int BeginAt { get; set; }
        
        public int ConfirmAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
