using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Modules.UserProfile;
using NetDream.Modules.UserProfile.Entities;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class ShippingRepository(ShopContext db, ProfileContext regionStore)
    {

        public ShippingListItem[] GetByAddress(AddressEntity address)
        {
            return GetByRegion(address.RegionId);
        }

        public ShippingListItem[] GetByRegion(int regionId)
        {
            var idItems = Array.Empty<int>();
            if (regionId > 0)
            {
                var regionIds = new RegionRepository(regionStore).GetPathId(regionId);
                idItems = db.ShippingRegions.Where(i => regionIds.Contains(i.RegionId))
                    .Pluck(i => i.GroupId);
            }
            
            var groups = db.ShippingGroups
                .When(idItems.Length > 0, i => idItems.Contains(i.Id) || i.IsAll == 1)
                .ToDictionary(i => i.ShippingId);
            if (groups.Count == 0)
            {
                return [];
            }
            var res = db.Shipping.Where(i => groups.Keys.Contains(i.Id))
                .ToArray();
            if (res.Length == 0)
            {
                return [];
            }
            return res.Select(i => new ShippingListItem()
            {
                Id = i.Id,
                Icon = i.Icon,
                Name = i.Name,
                Code  = i.Code,
                Description = i.Description,
                Settings = groups[i.Id],
            }).ToArray();
        }

        /**
         * 根据配送地址获取配送设置
         * @param int shipping
         * @param int region
         * @return array|null
         */
        public ShippingGroupEntity? GetGroup(int shipping, int region)
        {
            var groupId = db.ShippingRegions.Where(i => 
                i.ShippingId == shipping && i.RegionId == region)
                .Value(i => i.GroupId);
            if (groupId > 0)
            {
                return db.ShippingGroups.Where(i => i.Id == groupId)
                    .SingleOrDefault();
            }
            return db.ShippingGroups.Where(i => i.ShippingId == shipping && i.IsAll == 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 计算配送费
        /// </summary>
        /// <param name="shipping"></param>
        /// <param name="settings"></param>
        /// <param name="goods_list"></param>
        /// <returns></returns>
        public decimal GetFee(ShippingListItem shipping,
            ShippingGroupEntity settings, ICartItem[] goods_list)
        {
            var amount = 0;
            decimal price = 0;
            decimal weight = 0;
            foreach (var item in goods_list)
            {
                amount += item.Amount;
                price += item.Total;
                weight += item.Goods.Weight * item.Amount;
            }
            //instance = Manager.Shipping(shipping.Code);
            //return instance.Calculate(settings, amount, price, weight);
            return 0;
        }
    }
}
