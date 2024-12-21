
namespace NetDream.Modules.Shop.Entities
{
    
    public class AddressEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int RegionId { get; set; }
        
        public int UserId { get; set; }
        public string Tel { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
