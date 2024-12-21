
namespace NetDream.Modules.Shop.Entities
{
    
    public class OrderGoodsEntity
    {
        
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        
        public int GoodsId { get; set; }
        
        public int ProductId { get; set; }
        
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public string SeriesNumber { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        
        public int RefundId { get; set; }
        public int Status { get; set; }
        
        public int AfterSaleStatus { get; set; }
        
        public int CommentId { get; set; }
        
        public string TypeRemark { get; set; } = string.Empty;
    }
}
