
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Auth.Entities
{
    
    public class InviteLogEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int ParentId { get; set; }
        public byte Status { get; set; }
        public int CodeId { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
 
    }
}
