using NetDream.Modules.Shop.Entities;
using NetDream.Modules.UserProfile.Entities;
using NetDream.Shared.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Models
{
    public class OrderPreview : IOrder
    {
        public IList<IOrderProduct> Goods { get; set; } = [];

        public IOrderAddress? Address {  get; set; }
        public ICodeOptionItem? Payment { get; internal set; }
        public ICodeOptionItem? Shipping { get; internal set; }

        public decimal Discount { get; set; }

        public decimal PayFee { get; internal set; }
        public decimal ShippingFee { get; internal set; }
        public decimal GoodsAmount => Goods.Sum(i => i.Price * i.Amount);

        public decimal OrderAmount => GoodsAmount + PayFee + ShippingFee - Discount;

        public OrderPreview()
        {
            
        }

        public OrderPreview(ICartItem[] items)
        {
            
        }

        public void Add(AddressEntity model)
        {

        }

        public void Add(ICodeOptionItem model)
        {

        }
        public void Add(ICodeOptionItem model, ShippingGroupEntity group)
        {

        }
    }
}
