using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class OrderModel : OrderEntity
    {
        public string StatusLabel { get; set; }

        public OrderGoodsEntity[] GoodsList { get; set; }

        public OrderAddressEntity? Address { get; set; }

        public IUser? User { get; set; }

        public DeliveryEntity? Delivery { get; set; }
    }
}
