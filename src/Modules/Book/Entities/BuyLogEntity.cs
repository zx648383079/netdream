
namespace NetDream.Modules.Book.Entities
{
    
    public class BuyLogEntity
    {
        
        public int Id { get; set; }
        
        public int BookId { get; set; }
        
        public int ChapterId { get; set; }
        
        public int UserId { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
