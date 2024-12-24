using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Auth.Entities
{
    
    public class PermissionEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        
        public string DisplayName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        
        public int UpdatedAt { get; set; }

        
        public int CreatedAt { get; set; }

    }
}
