namespace NetDream.Modules.Shop.Backend.Models
{
    internal interface IWithGoodsModel
    {
        public int GoodsId { get; }
        public GoodsLabelItem? Goods { set; }
    }
}