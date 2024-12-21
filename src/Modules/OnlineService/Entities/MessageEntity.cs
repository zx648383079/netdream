
namespace NetDream.Modules.OnlineService.Entities
{
    
    public class MessageEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int SessionId { get; set; }
        
        public byte SendType { get; set; }
        public byte Type { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public string ExtraRule { get; set; } = string.Empty;
        public byte Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
