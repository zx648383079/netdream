using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Market.Models
{
    public class CollectListItem : CollectEntity, IWithGoodsModel
    {
        public GoodsListItem? Goods { get; set; }
    }
}
