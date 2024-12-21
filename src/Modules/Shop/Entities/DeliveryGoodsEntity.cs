
namespace NetDream.Modules.Shop.Entities
{
    
    public class DeliveryGoodsEntity
    {
        
        public int Id { get; set; }
        
        public int DeliveryId { get; set; }
        
        public int OrderGoodsId { get; set; }
        
        public int GoodsId { get; set; }
        
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        
        public string SeriesNumber { get; set; } = string.Empty;
        public int Amount { get; set; }
        
        public string TypeRemark { get; set; } = string.Empty;
    }
}
