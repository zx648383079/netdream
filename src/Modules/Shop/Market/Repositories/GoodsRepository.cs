using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    internal class GoodsRepository(ShopContext db, 
        IClientContext client,
        IGlobeOption option)
    {
        public IPage<GoodsListItem> Search(
            QueryForm form,
            int[] idItems, int category = 0, int brand = 0, 
            string price = "")
        {
            var (sort, order) = SearchHelper.CheckSortOrder(
                form.Sort, form.Order, ["id", "name", "price"], "desc");
            var query = db.Goods.When(idItems.Length > 0, i => idItems.Contains(i.Id))
                .Search(form.Keywords, "name")
                .When(category > 0, i => i.CatId == category)
                .When(brand > 0, i => i.BrandId == brand);
            new SearchRepository(db).FilterPrice(query, price);
            return query.OrderBy<GoodsEntity, int>(sort, order)
                .ToPage(form, AsList);
        }

        /**
         * 商品详情
         * @param int id
         * @return array|bool
         * @throws \Exception
         */
        public IOperationResult<GoodsModel> Detail(int id, 
            bool full = true, 
            int product = 0)
        {
            if (product > 0 && id < 1)
            {
                id = db.Products.Where(i => i.Id == product).Value(i => i.GoodsId);
            }
            var model = db.Goods.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<GoodsModel>("商品错误");
            }
            var res = model.CopyTo<GoodsModel>();
            //data = array_merge(data,
            //    GoodsMetaModel.GetOrDefault(id),
            //    AttributeRepository.BatchProperties(goods.AttributeGroupId, goods.Id)
            //);
            res.Category = db.Categories.Where(i => i.Id == model.CatId).SingleOrDefault();
            res.Brand = db.Brands.Where(i => i.Id == model.BrandId).SingleOrDefault();
            res.Products = db.Products.Where(i => i.GoodsId == model.Id).ToArray();
            res.IsCollect = client.UserId > 0 ? 
                db.Collects.Where(i => i.GoodsId == model.Id && i.UserId == client.UserId).Any() : false;
            res.Gallery = db.GoodsGalleries.Where(i => i.GoodsId == model.Id).ToArray();
            if (full)
            {
                res.Countdown = GetCountdown(model);
                res.Promotes = GetPromoteList(model);
                res.Coupons = GetCoupon(model);
                res.FavorableRate = new CommentRepository(db, client, null)
                    .FavorableRate(id);
                res.Services = [
                    "48小时快速退款",
                    "支持30天无忧退换货",
                    "满88元免邮费",
                    "自营品牌",
                    "国内部分地区无法配送"
                ];
            }
            return OperationResult.Ok(res);
        }

        public IOperationResult<GoodsEntity> Stock(int goodsId, int region = 0)
        {
            var model = db.Goods.Where(i => i.Id == goodsId)
                .SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<GoodsEntity>("商品错误");
            }
            // 判断库存
            // 判断快递是否支持
            return OperationResult.Ok(model);
        }

        /**
         * 查询价格和库存
         * @param int id
         * @param array properties
         * @param int amount
         * @param int region
         * @return array
         */
        public PriceResult Price(int id, 
            int[] properties, 
            int amount = 1, int region = 0)
        {
            var model = db.Goods.Where(i => i.Id == id)
                .Select(i => new GoodsEntity()
                {
                    Id = i.Id,
                    Price = i.Price,
                    Stock = i.Stock,
                })
                .SingleOrDefault();
            var price = FinalPrice(model, amount, properties);
            var box = new AttributeRepository(db).GetProductAndPriceWithProperties(
                properties, id);
            return new PriceResult()
            {
                Price = price,
                Total = price * amount,
                Stock = box.Product is not null ? box.Product.Stock : model.Stock
            };
        }

        public GoodsModel FormatProperties(GoodsEntity model)
        {
            var res = model.CopyTo<GoodsModel>();
            res.Properties = new AttributeRepository(db)
                .GetProperties(model.AttributeGroupId, model.Id);
            return res;
        }

        public IOperationResult<GoodsModel> GetDialog(int goods)
        {
            var model = db.Goods.Where(i => i.Id == goods).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<GoodsModel>("商品错误");
            }
            return OperationResult.Ok(FormatProperties(model));
        }

        public ProductCountdown GetCountdown(GoodsEntity goods)
        {
            return new ProductCountdown()
            {
                EndAt = TimeHelper.TimestampNow() + 3000,
                Name = "秒杀",
                Tip = "距秒杀结束还剩"
            };
        }

        public ActivityLabelItem[] GetPromoteList(GoodsEntity goods)
        {
            return new ActivityRepository(db).GoodsJoin(goods);
        }

        public CouponEntity[] GetCoupon(GoodsEntity goods)
        {
            return new CouponRepository(db, client).GoodsCanReceive(goods);
        }


        public bool CanBuy(int goods, int amount = 1,
            int[] properties = null)
        {
            var model = db.Goods.Where(i => i.Id == goods)
                .Select(i => new GoodsEntity()
                {
                    Id = i.Id,
                    Price = i.Price,
                    Stock = i.Stock,
                }).SingleOrDefault();
            return CanBuy(model, amount, properties);
        }
        /**
         * 判断是否能购买指定数量
         * @param GoodsModel|int goods
         * @param int amount
         * @param int[] properties
         * @return bool
         */
        public bool CanBuy(GoodsEntity goods, int amount = 1, 
            int[] properties = null)
        {
            if (properties is null || properties.Length == 0)
            {
                return CheckStock(goods, amount);
            }
            var box = new AttributeRepository(db)
                .GetProductAndPriceWithProperties(properties, goods.Id);
            if (box.Product is null)
            {
                return CheckStock(goods, amount);
            }
            return CheckStock(box.Product, amount);
        }
        public bool CheckStock(ProductEntity model, int amount = 0, int regionId = 0)
        {
            if (amount < 1)
            {
                return true;
            }
            if (regionId < 1 || option.Get<int>("shop_warehouse") < 1)
            {
                return model.Stock >= amount;
            }
            /// TODO ?
            return new WarehouseRepository(db, null).GetStock(regionId, model.GoodsId, model.Id) >= amount;
        }
        public bool CheckStock(GoodsEntity model, int amount = 0, int regionId = 0)
        {
            if (amount < 1)
            {
                return true;
            }
            if (regionId < 1 || option.Get<int>("shop_warehouse") < 1)
            {
                return model.Stock >= amount;
            }
            /// TODO ?
            return new WarehouseRepository(db, null).GetStock(regionId, model.Id, 0) >= amount;
        }
        public float FinalPrice(int goods, int amount = 1,
            int[] properties = null)
        {
            var model = CartRepository.GetGoods(goods);
            return FinalPrice(model, amount, properties);
        }
        /**
         * 获取最终价格
         * @param GoodsModel|int goods
         * @param int amount
         * @param array properties
         * @return float
         */
        public float FinalPrice(GoodsEntity goods, int amount = 1, 
            int[] properties = null)
        {
            if (properties is null || properties.Length == 0)
            {
                return goods.Price;
            }
            var box = new AttributeRepository(db)
                .GetProductAndPriceWithProperties(properties, goods.Id);
            if (box.Product is null)
            {
                return goods.Price + box.PropertiesPrice;
            }
            return box.Product.Price + box.PropertiesPrice;
        }


        /**
         * @param Query query
         * @param int category
         * @param int brand
         * @param null keywords
         * @param int per_page
         * @param id
         * @return Page
         * @throws \Exception
         */
        public IPage<GoodsListItem> AppendSearch(IQueryable<GoodsEntity> query,
            QueryForm form,
            int category = 0, int brand = 0,
            int[] idItems = null)
        {
            return query.When(idItems.Length > 0, i => idItems.Contains(i.Id))
                .Search(form.Keywords, "name")
                .When(category > 0, i => i.CatId == category)
                .When(brand > 0, i => i.BrandId == brand)
                .ToPage(form, AsList);
        }

        public HomeFloor HomeRecommend()
        {
            return new HomeFloor()
            {
                HotProducts = AsList(db.Goods.Where(i => i.IsHot == 1)).ToArray(),
                NewProducts = AsList(db.Goods.Where(i => i.IsNew == 1)).ToArray(),
                BestProducts = AsList(db.Goods.Where(i => i.IsBest == 1)).ToArray(),
            };
        }

        public void PaintShareImage(int id)
        {

        }

        internal static IQueryable<GoodsListItem> AsList(IQueryable<GoodsEntity> query)
        {
            return query.Select(i => new GoodsListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                MarketPrice = i.MarketPrice,
                CatId = i.CatId,
                BrandId = i.BrandId,
                Thumb = i.Thumb,
                Brief = i.Brief,
            });
        }

        internal static void Include(ShopContext db, IWithGoodsModel[] items)
        {
            var idItems = items.Select(item => item.GoodsId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = AsList(db.Goods.Where(i => idItems.Contains(i.Id)))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.GoodsId > 0 && data.TryGetValue(item.GoodsId, out var res))
                {
                    item.Goods = res;
                }
            }
        }
    }
}
