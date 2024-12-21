
namespace NetDream.Modules.Shop.Entities
{
    
    public class RegionEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int ParentId { get; set; }
        
        public string FullName { get; set; } = string.Empty;
    }
}
