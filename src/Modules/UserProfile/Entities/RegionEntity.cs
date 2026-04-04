using NetDream.Shared.Interfaces;

namespace NetDream.Modules.UserProfile.Entities
{
    
    public class RegionEntity : IIdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        
        public string FullName { get; set; } = string.Empty;
    }
}
