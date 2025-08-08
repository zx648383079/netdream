using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OpenPlatform.Entities
{
    
    public class PlatformOptionEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int PlatformId { get; set; }
        public string Store { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
