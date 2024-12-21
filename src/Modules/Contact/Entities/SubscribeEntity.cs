using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Contact.Entities
{
    
    public class SubscribeEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
