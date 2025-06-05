using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Shop.Market.Repositories
{
    internal class GoodsRepository(ShopContext db)
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
            return AsList(query.OrderBy<GoodsEntity, int>(sort, order)).ToPage(form);
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
            data = array_merge(data,
                GoodsMetaModel.GetOrDefault(id),
                AttributeRepository.BatchProperties(goods.AttributeGroupId, goods.Id)
            );
            unset(data["cost_price"]);
            data["category"] = goods.Category;
            data["brand"] = goods.Brand;
            data["products"] = goods.Products;
            data["is_collect"] = goods.IsCollect;
            data["gallery"] = goods.Gallery;
            if (full)
            {
                data["countdown"] = self.GetCountdown(goods);
                data["promotes"] = self.GetPromoteList(goods);
                data["coupons"] = self.GetCoupon(goods);
                data["favorable_rate"] = CommentRepository.FavorableRate(id);
                data["services"] = [
                    "48小时快速退款",
                "支持30天无忧退换货",
                "满88元免邮费",
                "自营品牌",
                "国内部分地区无法配送"
                ];
            }
            return data;
        }

        public static Stock(int goodsId, int region = 0)
        {
            goods = GoodsModel.FindOrThrow(goodsId, "商品已下架");
            // 判断库存
            // 判断快递是否支持
            return goods.ToArray();
        }

        /**
         * 查询价格和库存
         * @param int id
         * @param array properties
         * @param int amount
         * @param int region
         * @return array
         */
        public static array Price(int id, array properties = [], int amount = 1, int region = 0)
        {
            goods = GoodsModel.Where("id", id).First("id", "price", "stock");
            price = GoodsRepository.FinalPrice(goods, amount, properties);
            box = AttributeRepository.GetProductAndPriceWithProperties(properties, id);
            return [
                "price" => price,
            "total" => price * amount,
            "stock" => !empty(box["product"]) ? box["product"].Stock : goods.Stock
            ];
        }

        public static FormatProperties(GoodsModel goods)
        {
            data = goods.ToArray();
            unset(data["cost_price"]);
            data["properties"] = AttributeRepository.GetProperties(goods.AttributeGroupId, goods.Id);
            return data;
        }

        public static GetDialog(int goods)
        {
            return FormatProperties(GoodsDialogModel.FindOrThrow(goods));
        }

        public static GetCountdown(GoodsModel goods)
        {
            return [
                "end_at" => time() + 3000,
                "name" => "秒杀",
                "tip" => "距秒杀结束还剩"
            ];
        }

        public static GetPromoteList(GoodsModel goods)
        {
            return ActivityRepository.GoodsJoin(goods);
        }

        public static GetCoupon(GoodsModel goods)
        {
            return CouponRepository.GoodsCanReceive(goods);
        }

        /**
         * 判断是否能购买指定数量
         * @param GoodsModel|int goods
         * @param int amount
         * @param int[] properties
         * @return bool
         */
        public static CanBuy(object goods, int amount = 1, array properties = [])
        {
            if (is_numeric(goods))
            {
                goods = GoodsModel.Query().Where("id", goods)
                    .First("id", "price", "stock");
            }
            if (empty(properties))
            {
                return static.CheckStock(goods, amount);
            }
            box = AttributeRepository.GetProductAndPriceWithProperties(properties, goods.Id);
            if (empty(box["product"]))
            {
                return static.CheckStock(goods, amount);
            }
            return static.CheckStock(box["product"], amount);
        }

        public static bool CheckStock(ProductModel|GoodsEntity model, int amount = 0, int regionId = 0)
        {
            if (amount < 1)
            {
                return true;
            }
            if (regionId < 1 || Option.Value("shop_warehouse", 0) < 1)
            {
                return model.Stock >= amount;
            }
            goodsId = model.Id;
            productId = 0;
            if (model instanceof ProductModel) {
                goodsId = model.GoodsId;
                productId = model.Id;
            }
            return WarehouseRepository.GetStock(regionId, goodsId, productId) >= amount;
        }

        /**
         * 获取最终价格
         * @param GoodsModel|int goods
         * @param int amount
         * @param array properties
         * @return float
         */
        public static FinalPrice(GoodsModel|int goods, int amount = 1, array properties = [])
        {
            if (is_numeric(goods))
            {
                goods = CartRepository.GetGoods(intval(goods));
            }
            if (empty(properties))
            {
                return goods.Price;
            }
            box = AttributeRepository.GetProductAndPriceWithProperties(properties, goods.Id);
            if (empty(box["product"]))
            {
                return goods.Price + box["properties_price"];
            }
            return box["product"].Price + box["properties_price"];
        }

        public static Query GetRecommendQuery(string tag)
        {
            return GoodsSimpleModel.Where(tag, 1);
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
        public static Page AppendSearch(Query query, int category = 0, int brand = 0, string keywords = string.Empty, int per_page = 20, array id = [])
        {
            return query.When(!empty(id), use(id)(Query query) {
                query.WhereIn("id", array_map("intval", id));
            })
            .When(!empty(keywords), (Query query) {
                GoodsEntity.SearchWhere(query, "name");
            }).When(category > 0, use(category)(Query query) {
                query.Where("cat_id", intval(category));
            }).When(brand > 0, use(brand)(Query query) {
                query.Where("brand_id", intval(brand));
            }).Page(per_page);
        }

        public static array HomeRecommend()
        {
            hot_products = GoodsRepository.GetRecommendQuery("is_hot").All();
            new_products = GoodsRepository.GetRecommendQuery("is_new").All();
            best_products = GoodsRepository.GetRecommendQuery("is_best").All();
            return compact("hot_products", "new_products", "best_products");
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
