
namespace NetDream.Shared.Providers.Entities
{
    
    public class FileQuoteEntity
    {
        
        public int Id { get; set; }
        
        public int FileId { get; set; }
        
        public int ItemType { get; set; }
        
        public int ItemId { get; set; }
        
        public int UserId { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
