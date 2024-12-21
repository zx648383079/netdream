using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Blog.Entities
{
    
    public class LogEntity: IActionEntity
    {
        
        public int Id { get; set; }
        
        public byte ItemType { get; set; }
        
        public int ItemId { get; set; }
        
        public int UserId { get; set; }
        public byte Action { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
