using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Entities
{
    
    public class GroupEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
