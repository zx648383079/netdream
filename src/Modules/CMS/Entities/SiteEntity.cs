
namespace NetDream.Modules.CMS.Entities
{
    
    public class SiteEntity
    {
        
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        
        public byte MatchType { get; set; }
        
        public string MatchRule { get; set; } = string.Empty;
        
        public byte IsDefault { get; set; }
        public byte Status { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Options { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
