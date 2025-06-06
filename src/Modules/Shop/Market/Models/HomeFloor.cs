namespace NetDream.Modules.Shop.Market.Models
{
    public class HomeFloor
    {
        public GoodsListItem[] HotProducts { get; internal set; }
        public GoodsListItem[] NewProducts { get; internal set; }
        public GoodsListItem[] BestProducts { get; internal set; }
    }
}
