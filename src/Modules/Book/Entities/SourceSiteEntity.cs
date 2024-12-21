
namespace NetDream.Modules.Book.Entities
{
    
    public class SourceSiteEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
