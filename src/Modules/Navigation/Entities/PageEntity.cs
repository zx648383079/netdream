
namespace NetDream.Modules.Navigation.Entities
{
    
    public class PageEntity
    {
        
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        
        public string SiteId { get; set; } = string.Empty;
        public byte Score { get; set; }
        
        public int UserId { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
