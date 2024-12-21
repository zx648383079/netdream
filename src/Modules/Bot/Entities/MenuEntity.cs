
namespace NetDream.Modules.Bot.Entities
{
    
    public class MenuEntity
    {
        
        public int Id { get; set; }
        
        public int BotId { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
