using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Navigation.Entities
{
    
    public class CategoryEntity  : IIdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
    }
}
