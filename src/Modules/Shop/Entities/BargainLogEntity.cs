
namespace NetDream.Modules.Shop.Entities
{
    
    public class BargainLogEntity
    {
        
        public int Id { get; set; }
        
        public int BargainId { get; set; }
        
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
