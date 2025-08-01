
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineDisk.Entities
{
    
    public class ShareUserEntity : IIdEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int ShareId { get; set; }
    }
}
