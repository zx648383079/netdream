using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Bot.Entities
{
    
    public class UserGroupEntity : IIdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int BotId { get; set; }
        
        public string TagId { get; set; } = string.Empty;
    }
}
