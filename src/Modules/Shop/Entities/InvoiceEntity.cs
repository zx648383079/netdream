
namespace NetDream.Modules.Shop.Entities
{
    
    public class InvoiceEntity
    {
        
        public int Id { get; set; }
        
        public int TitleType { get; set; }
        public int Type { get; set; }
        public string Title { get; set; } = string.Empty;
        
        public string TaxNo { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Bank { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public decimal Money { get; set; }
        public int Status { get; set; }
        
        public int InvoiceType { get; set; }
        
        public string ReceiverEmail { get; set; } = string.Empty;
        
        public string ReceiverName { get; set; } = string.Empty;
        
        public string ReceiverTel { get; set; } = string.Empty;
        
        public int ReceiverRegionId { get; set; }
        
        public string ReceiverRegionName { get; set; } = string.Empty;
        
        public string ReceiverAddress { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
