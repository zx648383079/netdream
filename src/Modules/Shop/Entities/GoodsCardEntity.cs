
namespace NetDream.Modules.Shop.Entities
{
    
    public class GoodsCardEntity
    {
        
        public int Id { get; set; }
        
        public int GoodsId { get; set; }
        
        public string CardNo { get; set; } = string.Empty;
        
        public string CardPwd { get; set; } = string.Empty;
        
        public int OrderId { get; set; }
    }
}
