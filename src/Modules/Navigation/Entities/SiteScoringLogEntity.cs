
namespace NetDream.Modules.Navigation.Entities
{
    
    public class SiteScoringLogEntity
    {
        
        public int Id { get; set; }
        
        public int SiteId { get; set; }
        
        public int UserId { get; set; }
        public byte Score { get; set; }
        
        public byte LastScore { get; set; }
        
        public string ChangeReason { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
