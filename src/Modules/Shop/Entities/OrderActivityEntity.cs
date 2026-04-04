using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Shop.Entities
{
    
    public class OrderActivityEntity : IIdEntity
    {
        
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        
        public int ProductId { get; set; }
        public byte Type { get; set; }
        public decimal Amount { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
