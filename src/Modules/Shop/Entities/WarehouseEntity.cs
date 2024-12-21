
namespace NetDream.Modules.Shop.Entities
{
    
    public class WarehouseEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        
        public string LinkUser { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
