
namespace NetDream.Modules.Shop.Entities
{
    
    public class OrderActivityEntity
    {
        
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        
        public int ProductId { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
