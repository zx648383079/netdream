
namespace NetDream.Modules.Chat.Entities
{
    
    public class FriendClassifyEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
