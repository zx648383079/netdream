
namespace NetDream.Modules.Book.Entities
{
    
    public class ListItemEntity
    {
        
        public int Id { get; set; }
        
        public int ListId { get; set; }
        
        public int BookId { get; set; }
        public string Remark { get; set; } = string.Empty;
        public int Star { get; set; }
        
        public int AgreeCount { get; set; }
        
        public int DisagreeCount { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
