
namespace NetDream.Modules.Shop.Entities
{
    
    public class AuctionLogEntity
    {
        
        public int Id { get; set; }
        
        public int ActId { get; set; }
        
        public int UserId { get; set; }
        public decimal Bid { get; set; }
        public int Amount { get; set; }
        public int Status { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
