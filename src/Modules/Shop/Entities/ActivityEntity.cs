
namespace NetDream.Modules.Shop.Entities
{
    
    public class ActivityEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Type { get; set; }
        
        public int ScopeType { get; set; }
        public string Scope { get; set; } = string.Empty;
        public string Configure { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int StartAt { get; set; }
        
        public int EndAt { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
