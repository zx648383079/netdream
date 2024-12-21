
namespace NetDream.Modules.Shop.Entities
{
    
    public class CartEntity
    {
        
        public int Id { get; set; }
        public int Type { get; set; }
        
        public int UserId { get; set; }
        
        public int GoodsId { get; set; }
        
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        
        public int IsChecked { get; set; }
        
        public int SelectedActivity { get; set; }
        
        public string AttributeId { get; set; } = string.Empty;
        
        public string AttributeValue { get; set; } = string.Empty;
        
        public int ExpiredAt { get; set; }
    }
}
