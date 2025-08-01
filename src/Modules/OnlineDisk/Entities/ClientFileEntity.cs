using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineDisk.Entities
{
    
    public class ClientFileEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Md5 { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Size { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
