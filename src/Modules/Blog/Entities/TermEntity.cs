using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Blog.Entities
{
    
    public class CategoryEntity : IIdEntity
    {
        
        public int Id { get; set; }

        public string EnName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int ParentId { get; set; }
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Styles { get; set; } = string.Empty;
        
    }
}
