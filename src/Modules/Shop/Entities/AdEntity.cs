
namespace NetDream.Modules.Shop.Entities
{
    
    public class AdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int PositionId { get; set; }
        public int Type { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        
        public int StartAt { get; set; }
        
        public int EndAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
