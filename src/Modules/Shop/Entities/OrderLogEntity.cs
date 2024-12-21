
namespace NetDream.Modules.Shop.Entities
{
    
    public class OrderLogEntity
    {
        
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        
        public int UserId { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
