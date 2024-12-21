
namespace NetDream.Modules.Shop.Entities
{
    
    public class AdPositionEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Width { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
