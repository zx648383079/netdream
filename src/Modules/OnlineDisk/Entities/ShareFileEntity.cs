using NetDream.Shared.Interfaces;

namespace NetDream.Modules.OnlineDisk.Entities
{
    
    public class ShareFileEntity : IIdEntity
    {
        
        public int Id { get; set; }
        
        public int DiskId { get; set; }
        
        public int ShareId { get; set; }
    }
}
