using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class OrderListItem : OrderEntity, IWithUserModel
    {
        public IUser? User { get; set; }

        public OrderGoodsEntity[] Goods { get; set; }

        public OrderAddressEntity? Address { get; set; }
    }
}
