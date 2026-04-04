using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Shop.Entities
{
    
    public class AttributeGroupEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public string PropertyGroups { get; set; } = string.Empty;

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
    }
}
