
namespace NetDream.Modules.Navigation.Entities
{
    
    public class SiteEntity
    {
        
        public int Id { get; set; }
        public string Schema { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public int CatId { get; set; }
        
        public int UserId { get; set; }
        
        public byte TopType { get; set; }
        public byte Score { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
