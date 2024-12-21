
namespace NetDream.Modules.Blog.Entities
{
    
    public class TagRelationshipEntity
    {
        
        
        public int TagId { get; set; }
        
        public int BlogId { get; set; }
        public int Position { get; set; }
    }
}
