
namespace NetDream.Modules.Book.Entities
{
    
    public class SourceEntity
    {
        
        public int Id { get; set; }
        
        public int BookId { get; set; }
        
        public int SizeId { get; set; }
        public string Url { get; set; } = string.Empty;
        
        public int DeletedAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
