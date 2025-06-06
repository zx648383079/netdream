using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Models;
using System;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class ActivityRepository(ShopContext db)
    {
        internal ActivityLabelItem[] GoodsJoin(GoodsEntity goods)
        {
            throw new NotImplementedException();
        }
    }
}