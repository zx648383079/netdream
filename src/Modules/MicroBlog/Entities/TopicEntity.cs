using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.MicroBlog.Entities
{
    
    public class TopicEntity : IIdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
