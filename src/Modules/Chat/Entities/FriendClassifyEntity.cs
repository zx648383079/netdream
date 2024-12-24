
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Entities
{
    
    public class FriendClassifyEntity: IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
