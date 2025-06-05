using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Market.Models
{
    public class OrderListItem : OrderEntity
    {
        public OrderGoodsEntity[] Goods { get; set; }
    }

}
