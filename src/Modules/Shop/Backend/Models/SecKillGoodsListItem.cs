using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class SecKillGoodsListItem : SecKillGoodsEntity, IWithGoodsModel
    {
        public GoodsLabelItem? Goods { get; set; }
    }
}
