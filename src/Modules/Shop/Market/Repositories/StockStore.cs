using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Modules.Shop.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class StockStore(ShopContext db, IGlobeOption option)
    {
        public const byte STATUS_NONE = 0;
        /**
         * 下单
         */
        public const byte STATUS_ORDER = 1;
        /**
         * 支付
         */
        public const byte STATUS_PAY = 2;
        /**
         * 发货
         */
        public const byte STATUS_SHIPPING = 2;


        private readonly Dictionary<int, Dictionary<int, int>> _data = [];
        private readonly int _stockTime = option.Get<int>("shop_store");
        private readonly bool _useWarehouse = option.Get<int>("shop_warehouse") > 0;
        public int OrderStatus { get; set; } = STATUS_NONE;
        public int Region {  get; set; }
        /// <summary>
        /// 判断当前状态是否影响库存
        /// </summary>
        public bool IsImpactInventory => _stockTime > 0 && OrderStatus == _stockTime;

        /// <summary>
        /// 冻结库存
        /// </summary>
        /// <param name="goods_list"></param>
        /// <returns></returns>
        public bool Frozen(ICartItem[] goods_list)
        {
            var success = true;
            foreach (var item in goods_list)
            {
                if (!FrozenItem(item.GoodsId, item.ProductId, item.Amount))
                {
                    success = false;
                    break;
                }
            }
            if (!success)
            {
                Restore();
            }
            return success;
        }


        /// <summary>
        /// 冻结库存
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="product_id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool FrozenItem(int goods_id, int product_id, int amount)
        {
            if (amount < 1 || !IsImpactInventory)
            {
                return true;
            }
            var store = Get(goods_id, product_id);
            if (store < amount)
            {
                return false;
            }
            Update(goods_id, product_id, -amount);
            PushStock(goods_id, product_id, amount);
            return true;
        }

        /// <summary>
        /// 判读库存是否充足
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="product_id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Check(int goods_id, int product_id, int amount)
        {
            if (amount < 1 || _stockTime < 1)
            {
                return true;
            }
            return Get(goods_id, product_id) >= amount;
        }

        /**
         * 获取库存
         * @param int goods_id
         * @param int product_id
         * @return int
         */
        public int Get(int goods_id, int product_id)
        {
            if (_useWarehouse && Region > 0)
            {
                return new WarehouseRepository(db, null)
                    .GetStock(Region, goods_id, product_id);
            }
            if (product_id > 0)
            {
                return db.Products.Where(i => i.Id == product_id && i.GoodsId == goods_id)
                    .Value(i => i.Stock);
            }
            return db.Goods.Where(i => i.Id == goods_id && i.Status == GoodsStatus.STATUS_SALE)
                .Value(i => i.Stock);
        }

        public void Update(int goods_id, int product_id, int amount)
        {
            if (amount == 0)
            {
                return;
            }
            if (_useWarehouse && Region > 0)
            {
                new WarehouseRepository(db, null)
                    .UpdateStock(Region, goods_id, product_id, amount);
                return;
            }
            if (product_id > 0)
            {
                db.Products.Where(i => i.Id == product_id && i.GoodsId == goods_id)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Stock, i => i.Stock + amount));
            }
            else
            {
                db.Goods.Where(i => i.Id == goods_id)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Stock, i => i.Stock + amount));
            }
        }

        /**
         * 清空，表示下单成功
         * @return bool
         */
        public bool Clear()
        {
            _data.Clear();
            return true;
        }

        /**
         * 下单失败，回滚
         * @return bool
         * @throws \Exception
         */
        public bool Restore()
        {
            foreach (var items in _data)
            {
                foreach (var item in items.Value)
                {
                    Update(items.Key, item.Key, item.Value);
                    items.Value[item.Key] = 0;
                }
            }
            _data.Clear();
            return true;
        }

        /**
         * 手动设置冻结的库存
         * @param int goods_id
         * @param int product_id
         * @param int amount
         * @return void
         */
        public void PushStock(int goods_id, int product_id, int amount)
        {
            if (!_data.TryGetValue(goods_id, out var items))
            {
                items = [];
                _data.Add(goods_id, items);
            }
            if (!items.TryAdd(product_id, amount))
            {
                items[product_id] += amount;
            }
        }
    }
}
