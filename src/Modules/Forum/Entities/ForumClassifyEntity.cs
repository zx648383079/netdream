
namespace NetDream.Modules.Forum.Entities
{
    
    public class ForumClassifyEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        
        public int ForumId { get; set; }
        public byte Position { get; set; }
    }
}
