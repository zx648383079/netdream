using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class CartEntity : IIdEntity
    {
        
        public int Id { get; set; }
        public byte Type { get; set; }
        
        public int UserId { get; set; }
        
        public int GoodsId { get; set; }
        
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        
        public bool IsChecked { get; set; }
        
        public int SelectedActivity { get; set; }
        
        public string AttributeId { get; set; } = string.Empty;
        
        public string AttributeValue { get; set; } = string.Empty;
        
        public int ExpiredAt { get; set; }
    }
}
