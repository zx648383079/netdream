using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Navigation.Entities
{
    
    public class PageEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        
        public int SiteId { get; set; }
        public byte Score { get; set; }
        
        public int UserId { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
