using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Document.Entities
{
    
    public class ProjectEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public uint CatId { get; internal set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Cover { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int DeletedAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
 
    }
}
