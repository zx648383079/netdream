using NetDream.Modules.Shop.Market.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class ShippingCart(ICartSource db) : List<ICartItem>, IList<ICartItem>
    {

        public float Total => this.Sum(i => i.Total);

        public int Amount => this.Sum(i => i.Amount);

        public bool TryAdd(int goodsId, int[] properties, int amount)
        {
            return false;
        }

        public bool TryUpdate(int goodsId, int[] properties, int amount)
        {
            return false;
        }

        public bool TryUpdate(int id, int amount)
        {
            return false;
        }

        public bool TryGet(int id, [NotNullWhen(true)] out ICartItem? result)
        {
            result = null;
            return false;
        }
        public bool TryGet(int goodsId, int[] properties, [NotNullWhen(true)] out ICartItem? result)
        {
            result = null;
            return false;
        }

        public bool TryRemove(int id)
        {
            return false;
        }
    }
}
