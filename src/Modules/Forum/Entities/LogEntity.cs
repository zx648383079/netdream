
namespace NetDream.Modules.Forum.Entities
{
    
    public class LogEntity
    {
        
        public int Id { get; set; }
        
        public byte ItemType { get; set; }
        
        public int ItemId { get; set; }
        
        public int UserId { get; set; }
        public int Action { get; set; }
        public string Data { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
