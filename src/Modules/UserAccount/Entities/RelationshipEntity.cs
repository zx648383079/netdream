using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.UserAccount.Entities
{
    
    public class RelationshipEntity: ICreatedEntity
    {
        public int UserId { get; set; }
        
        public int LinkId { get; set; }
        public int Type { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
