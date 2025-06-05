using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Market.Models
{
    public class OrderModel : OrderEntity
    {
        public OrderAddressEntity Address { get; set; }
        public OrderGoodsEntity[] Goods { get; set; }
        public int ExpiredAt { get; set; }
    }
}
