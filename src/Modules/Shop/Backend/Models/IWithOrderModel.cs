using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Backend.Models
{
    internal interface IWithOrderModel
    {
        public int OrderId { get; }

        public OrderEntity? Order { set; }
    }
}