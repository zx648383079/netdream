
namespace NetDream.Modules.Book.Entities
{
    
    public class BookEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public int AuthorId { get; set; }
        
        public int UserId { get; set; }
        public int Classify { get; set; }
        
        public int CatId { get; set; }
        public int Size { get; set; }
        
        public int ClickCount { get; set; }
        
        public int RecommendCount { get; set; }
        
        public int OverAt { get; set; }
        public int Status { get; set; }
        
        public int SourceType { get; set; }
        
        public int DeletedAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
