using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Document.Entities
{
    
    public class FieldEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        
        public bool IsRequired { get; set; }
        
        public string DefaultValue { get; set; } = string.Empty;
        public string Mock { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        
        public int ApiId { get; set; }
        public int Kind { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
