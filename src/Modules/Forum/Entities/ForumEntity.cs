using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Forum.Entities
{
    
    public class ForumEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public int ParentId { get; set; }

        public int ZoneId { get; set; }

        public int ThreadCount { get; set; }
        
        public int PostCount { get; set; }
        public byte Type { get; set; }
        public byte Position { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
