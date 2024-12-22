
namespace NetDream.Shared.Providers.Entities
{
    
    public class MetaEntity
    {
        
        public int Id { get; set; }
        
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}