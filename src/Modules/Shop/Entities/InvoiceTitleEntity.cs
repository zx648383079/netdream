
namespace NetDream.Modules.Shop.Entities
{
    
    public class InvoiceTitleEntity
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
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
