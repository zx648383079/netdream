using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Document.Entities
{
    
    public class ProjectVersionEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
