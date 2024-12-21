
namespace NetDream.Modules.Shop.Entities
{
    
    public class BargainUserEntity
    {
        
        public int Id { get; set; }
        
        public int ActId { get; set; }
        
        public int UserId { get; set; }
        
        public int GoodsId { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
