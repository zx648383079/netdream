
namespace NetDream.Modules.Book.Entities
{
    
    public class ChapterEntity
    {
        
        public int Id { get; set; }
        
        public int BookId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        public int Price { get; set; }
        public int Status { get; set; }
        public int Position { get; set; }
        public int Size { get; set; }
        
        public int DeletedAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
