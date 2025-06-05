namespace NetDream.Modules.Shop.Market.Models
{
    internal interface IWithGoodsModel
    {
        public int GoodsId { get; }
        public GoodsListItem? Goods { set; }
    }
}