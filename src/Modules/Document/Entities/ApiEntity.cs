using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Document.Entities
{
    
    public class ApiEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;
        
        public int ProjectId { get; set; }
        
        public int VersionId { get; set; }
        public byte Type { get; set; }
        public string Description { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
