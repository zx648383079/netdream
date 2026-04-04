using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Document.Entities
{
    
    public class PageEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int ProjectId { get; set; }
        
        public int VersionId { get; set; }
        
        public int ParentId { get; set; }
        public byte Type { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
