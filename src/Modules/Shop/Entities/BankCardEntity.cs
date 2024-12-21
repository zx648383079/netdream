
namespace NetDream.Modules.Shop.Entities
{
    
    public class BankCardEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public string Bank { get; set; } = string.Empty;
        public int Type { get; set; }
        
        public string CardNo { get; set; } = string.Empty;
        
        public string ExpiryDate { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
