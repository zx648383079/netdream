using NetDream.Modules.Shop.Models;

namespace NetDream.Modules.Shop.Market.Models
{
    public class CategoryFloorItem : TreeListItem
    {
        public GoodsListItem[] GoodsList { get; internal set; }
        public string Url { get; internal set; }
    }
}
