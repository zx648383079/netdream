
namespace NetDream.Modules.Shop.Entities
{
    
    public class BrandEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        
        public string AppLogo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
