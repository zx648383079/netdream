using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class ArticleCategoryEntity: IIdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        public int Position { get; set; }
    }
}
