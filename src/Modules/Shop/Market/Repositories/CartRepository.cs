using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Modules.Shop.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class CartRepository(ShopContext db,
        IClientContext client)
        : ICartSource
    {
        public ShippingCart Instance => new(this);

        public GoodsEntity? GetGoods(int goodId)
        {
            return db.Goods.Where(i => i.Id == goodId)
                .Select(i => new GoodsEntity()
                {
                    Id = i.Id,
                    Name = i.Name,
                    SeriesNumber = i.SeriesNumber,
                    Thumb = i.Thumb,
                    Price = i.Price,
                    Weight = i.Weight,
                    Stock = i.Stock,
                    MarketPrice = i.MarketPrice,
                    CatId = i.CatId,
                    BrandId = i.BrandId,
                    Status = i.Status,
                })
                .SingleOrDefault();
        }
        public ICartItem FormatCartItem(int goodsId, string properties, int amount)
        {
            return FormatCartItem(goodsId, AttributeRepository.FormatPostProperties(properties), amount);
        }
        public ICartItem FormatCartItem(int goodsId, string[] properties, int amount)
        {
            return FormatCartItem(goodsId, AttributeRepository.FormatPostProperties(properties), amount);
        }
        public ICartItem FormatCartItem(int goodsId, int[] properties, int amount)
        {
            var box = new AttributeRepository(db, null)
                .GetProductAndPriceWithProperties(properties, goodsId);
            return new CartItem()
            {
                GoodsId = goodsId,
                ProductId = box.Product?.Id ?? 0,
                Amount = amount,
                Price = new GoodsRepository(db, client, null).FinalPrice(goodsId, amount, properties),
                AttributeId = string.Join(',', properties),
                AttributeValue = box.PropertiesLabel
            };
        }

        /**
         *
         * @param goods
         * @param int amount
         * @return GoodsModel
         * @throws Exception
         */
        public IOperationResult<GoodsEntity> CheckGoods(int goods, int amount = 1)
        {
            var model = db.Goods.Where(i => i.Id == goods).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<GoodsEntity>.Fail("商品不存在");
            }
            if (model.Status != GoodsStatus.STATUS_SALE)
            {
                return OperationResult<GoodsEntity>.Fail($"商品【{model.Name}】已下架");
            }
            if (!new GoodsRepository(db, client, null).CanBuy(model, amount))
            {
                return OperationResult<GoodsEntity>.Fail($"商品【{model.Name}】库存不足");
            }
            return OperationResult.Ok(model);
        }

        /**
         * 验证商品属性是否正确
         * @param id
         * @param int amount
         * @param int[] properties
         * @return array [GoodsModel, ProductModel, 规格是否对]
         * @throws Exception
         */
        public IOperationResult<ProductVerifyResult> CheckGoodsOrProduct(int id, int amount = 1,
            int[] properties = null)
        {
            var model = GetGoods(id);
            if (model is null)
            {
                return OperationResult<ProductVerifyResult>.Fail("商品不存在");
            }
            if (model.Status != GoodsStatus.STATUS_SALE)
            {
                return OperationResult<ProductVerifyResult>.Fail($"商品【{model.Name}】已下架");
            }
            var box = new AttributeRepository(db, null)
                .GetProductAndPriceWithProperties(properties, model.Id);
            if (box.Product is null 
                && db.Products.Where(i => i.GoodsId == model.Id).Any())
            {
                return OperationResult.Ok(new ProductVerifyResult()
                {
                    Goods = model,
                    PropertyIsValid = false
                });
            }
            if (!new GoodsRepository(db, client, null)
                .CanBuy(model, amount, properties))
            {
                return OperationResult<ProductVerifyResult>.Fail($"商品【{model.Name}】库存不足");
            }
            return OperationResult.Ok(new ProductVerifyResult()
            {
                Goods = model,
                Product = box.Product,
                PropertyIsValid = false
            });
        }

        public bool AddGoods(int goods, int amount = 1, int[] properties = null)
        {
            var cart = Instance;
            return cart.TryAdd(goods, properties, amount);
        }

        public bool UpdateGoods(int goods, int amount = 1, int[] properties = null)
        {
            var cart = Instance;
            return cart.TryUpdate(goods, properties, amount);
        }

        public bool UpdateAmount(int id, int amount = 1)
        {
            var cart = Instance;
            return cart.TryUpdate(id, amount);
        }

        /**
         * 清空失效的商品
         * @return Cart
         * @throws Exception
         */
        public ShippingCart RemoveInvalid()
        {
            return Instance;
        }

        public bool Remove(int id)
        {
            var cart = Instance;
            return cart.TryRemove(id);
        }

        public ICartItem[] Load()
        {
            var items = db.Carts.Where(i => i.UserId == client.UserId).ToArray();
            if (items.Length == 0)
            {
                return [];
            }
            var idItems = items.Select(i => i.GoodsId).ToArray();
            var data = db.Goods.Where(i => idItems.Contains(i.Id))
                .Select(i => new GoodsEntity()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Thumb = i.Thumb,
                    Price = i.Price,
                    Stock = i.Stock,
                    MarketPrice = i.MarketPrice,
                }).ToDictionary(i => i.Id);
            return items.Select(i => new CartItem()
            {
                Id = i.Id,
                Amount = i.Amount,
                GoodsId = i.GoodsId,
                ProductId = i.ProductId,
                SelectedActivity = i.SelectedActivity,
                IsChecked = i.IsChecked,
                Goods = data[i.GoodsId]
            }).ToArray();
        }

        public void Save(ICartItem[] items)
        {

        }
    }
}
