
namespace NetDream.Modules.Book.Entities
{
    
    public class HistoryEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int BookId { get; set; }
        
        public int ChapterId { get; set; }
        public double Progress { get; set; }
        
        public int SourceId { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
