
namespace NetDream.Modules.Bot.Entities
{
    
    public class UserGroupEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int BotId { get; set; }
        
        public string TagId { get; set; } = string.Empty;
    }
}
