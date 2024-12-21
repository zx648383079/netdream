
namespace NetDream.Modules.Chat.Entities
{
    
    public class HistoryEntity
    {
        
        public int Id { get; set; }
        
        public int ItemType { get; set; }
        
        public int ItemId { get; set; }
        
        public int UserId { get; set; }
        
        public int UnreadCount { get; set; }
        
        public int LastMessage { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
