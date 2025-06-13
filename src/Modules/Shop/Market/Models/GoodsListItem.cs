using NetDream.Modules.Shop.Models;

namespace NetDream.Modules.Shop.Market.Models
{
    public class GoodsListItem : IGoodsItem
    {
        public int Id { get; set; }

        public int CatId { get; set; }

        public int BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal MarketPrice { get; set; }
    }
}
