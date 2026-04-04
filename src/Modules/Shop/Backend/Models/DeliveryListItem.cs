using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class DeliveryListItem : DeliveryEntity, IWithUserModel, IWithOrderModel
    {
        public IUser? User { get; set; }

        public DeliveryGoodsEntity[]? Goods { get; set; }
        public OrderEntity? Order { get; set; }
    }
}
