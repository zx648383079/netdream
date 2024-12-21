
namespace NetDream.Modules.Shop.Entities
{
    
    public class CategoryEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Banner { get; set; } = string.Empty;
        
        public string AppBanner { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        public int Position { get; set; }
    }
}
