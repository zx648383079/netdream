using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Shop.Market.Models
{
    public class CartItem : ICartItem
    {
        public int Amount {  get; set; }

        public float Price { get; set; }

        public float Total => Amount * Price;

        public int GoodsId { get; internal set; }
        public int ProductId { get; internal set; }
        public string AttributeId { get; internal set; }
        public string AttributeValue { get; internal set; }

        public IGoodsSource Goods { get; set; }

    }
}
