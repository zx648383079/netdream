using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class GoodsRepository(
        ShopContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        const char ATTRIBUTE_LINK = ',';
        public IPage<GoodsListItem> GetList(QueryForm form, int[] idItems, 
            int category = 0, int brand = 0, 
            bool trash = false)
        {
            var (sort, order) = SearchHelper.CheckSortOrder(form.Sort, form.Order, 
                ["id", "name", "price", "stock"], "desc");
            var res = db.Goods
                .When(idItems.Length > 0, i => idItems.Contains(i.Id))
                .Search(form.Keywords, "name")
                .When(category > 0, i => i.CatId == category)
                .When(brand > 0, i => i.BrandId == brand)
                .When(trash, i => i.DeletedAt > 0)
                .OrderBy<GoodsEntity, int>(sort, order)
                .ToPage(form, AsList);
            CategoryRepository.Include(db, res.Items);
            BrandRepository.Include(db, res.Items);
            return res;
        }
        public IOperationResult<GoodsEntity> Get(int id)
        {
            var model = db.Goods.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<GoodsEntity>.Fail("商品不存在");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<GoodsModel> GetFull(int id)
        {
            var model = db.Goods.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<GoodsModel>.Fail("商品不存在");
            }
            var res = model.CopyTo<GoodsModel>();
            res.Gallery = db.GoodsGalleries.Where(i => i.GoodsId == model.Id).ToArray();
            res.MetaItems = db.GoodsMetas.Where(i => i.GoodsId == model.Id)
                .Select(i => new KeyValuePair<string, string>(i.Name, i.Content))
                .ToDictionary();
            return OperationResult.Ok(res);
        }

        public IOperationResult<GoodsEntity> Save(GoodsForm data)
        {
            var model = data.Id > 0 ?
                db.Goods.Where(i => i.Id == data.Id).SingleOrDefault()
                : new GoodsEntity();
            if (model == null)
            {
                return OperationResult<GoodsEntity>.Fail("数据错误");
            }
            model.Name = data.Name;
            db.Goods.Save(model, client.Now);
            db.SaveChanges();
            if (data.Id == 1)
            {
                db.GoodsAttributes.Where(i => i.GoodsId == 0)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.GoodsId, model.Id));
            }
            // GoodsMetaModel.SaveBatch(model.Id, data);
            if (data.Attr is not null)
            {
                BatchSaveAttribute(data.Attr, model.Id, model.AttributeGroupId);
            }
            if (data.Products is not null)
            {
                BatchSaveProduct(data.Products, model.Id);
            }
            if (data.Gallery is not null)
            {
                BatchSaveGallery(data.Gallery, model.Id);
            }
            return OperationResult.Ok(model);
        }

        public void BatchSaveProduct(ProductForm[] data, int goodsId)
        {
            var exist = new List<int>();
            foreach (var item in data)
            {
                var attributes = new List<int>();
                foreach (var label in item.Attributes.Split(ATTRIBUTE_LINK))
                {
                    var attrId = db.GoodsAttributes.Where(i => i.Value == label && i.GoodsId == goodsId)
                        .Value(i => i.Id);
                    if (attrId < 1)
                    {
                        continue;
                    }
                    attributes.Add(attrId);
                }
                item.Attributes = string.Join(ATTRIBUTE_LINK, attributes.Order());
                item.GoodsId = goodsId;
                var r = ProductSave(item);
                if (r.Succeeded)
                {
                    exist.Add(r.Result.Id);
                }
            }
            db.Products.Where(i => i.GoodsId == goodsId && !exist.Contains(i.Id))
                .ExecuteDelete();
        }

        public IOperationResult<ProductEntity> ProductSave(ProductForm data)
        {
            var model = data.Id > 0 ?  
                db.Products.Where(i => i.Id == data.Id).SingleOrDefault()
                : new ProductEntity();
            if (model == null)
            {
                return OperationResult<ProductEntity>.Fail("数据错误");
            }
            model.SeriesNumber = data.SeriesNumber;
            model.GoodsId = data.GoodsId;
            model.MarketPrice = data.MarketPrice;
            model.Price = data.Price;
            model.Weight = data.Weight;
            model.Attributes = data.Attributes;
            model.Stock = data.Stock;
            db.Products.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void BatchSaveAttribute(GoodsAttributeForm[] data, int goodsId, int groupId)
        {
            var allow = db.Attributes.Where(i => i.GroupId == groupId)
                .Pluck(i => i.Id);
            if (allow.Length == 0)
            {
                return;
            }
            var attrId = new List<int>();
            foreach (var item in data)
            {
                if (!allow.Contains(item.AttributeId))
                {
                    continue;
                }
                item.GoodsId = goodsId;
                var r = AttributeSave(item);
                if (r.Succeeded)
                {
                    attrId.Add(r.Result.Id);
                }
            }
            db.GoodsAttributes.Where(i => i.GoodsId == goodsId && !attrId.Contains(i.Id))
                .ExecuteDelete();
        }

        public IOperationResult<GoodsAttributeEntity> AttributeSave(GoodsAttributeForm data)
        {
            var model = data.Id > 0 ?
                db.GoodsAttributes.Where(i => i.Id == data.Id).SingleOrDefault()
                : new GoodsAttributeEntity();
            if (model == null)
            {
                return OperationResult<GoodsAttributeEntity>.Fail("数据错误");
            }
            model.GoodsId = data.GoodsId;
            model.Value = data.Value;
            model.AttributeId = data.AttributeId;
            model.Price = data.Price;
            db.GoodsAttributes.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void BatchSaveGallery(GoodsGalleryForm[] files, int goodsId)
        {
            var fileId = new List<int>();
            foreach (var item in files)
            {
                item.GoodsId = goodsId;
                var r = GallerySave(item);
                if (r.Succeeded)
                {
                    fileId.Add(r.Result.Id);
                }
            }
            db.GoodsGalleries.Where(i => i.GoodsId == goodsId && !fileId.Contains(i.Id))
                .ExecuteDelete();
        }

        public IOperationResult<GoodsGalleryEntity> GallerySave(GoodsGalleryForm data)
        {
            var model = data.Id > 0 ?
                db.GoodsGalleries.Where(i => i.Id == data.Id).SingleOrDefault()
                : new GoodsGalleryEntity();
            if (model == null)
            {
                return OperationResult<GoodsGalleryEntity>.Fail("数据错误");
            }
            model.GoodsId = data.GoodsId;
            model.Thumb = data.Thumb;
            model.Type = data.Type;
            model.File = data.File;
            db.GoodsGalleries.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id, bool trash = false)
        {
            if (!trash)
            {
                db.Goods.Where(i => i.Id == id)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.DeletedAt, client.Now
                   ));
                db.SaveChanges();
                return;
            }
            db.Goods.Where(i => i.DeletedAt > 0 && i.Id == id).ExecuteDelete();
            db.GoodsMetas.Where(i => i.GoodsId == i.Id).ExecuteDelete();
            db.GoodsCards.Where(i => i.GoodsId == i.Id).ExecuteDelete();
            db.GoodsIssues.Where(i => i.GoodsId == i.Id).ExecuteDelete();
            db.GoodsAttributes.Where(i => i.GoodsId == i.Id).ExecuteDelete();
            db.GoodsGalleries.Where(i => i.GoodsId == i.Id).ExecuteDelete();
            db.Carts.Where(i => i.GoodsId == i.Id).ExecuteDelete();
            db.SaveChanges();
        }

        public void ClearTrash()
        {
            var goodsIds = db.Goods.Where(i => i.DeletedAt > 0)
                .Pluck(i => i.Id);
            if (goodsIds.Length == 0)
            {
                return;
            }
            db.Goods.Where(i => goodsIds.Contains(i.Id)).ExecuteDelete();
            db.GoodsMetas.Where(i => goodsIds.Contains(i.GoodsId)).ExecuteDelete();
            db.GoodsCards.Where(i => goodsIds.Contains(i.GoodsId)).ExecuteDelete();
            db.GoodsIssues.Where(i => goodsIds.Contains(i.GoodsId)).ExecuteDelete();
            db.GoodsAttributes.Where(i => goodsIds.Contains(i.GoodsId)).ExecuteDelete();
            db.GoodsGalleries.Where(i => goodsIds.Contains(i.GoodsId)).ExecuteDelete();
            db.Carts.Where(i => goodsIds.Contains(i.GoodsId)).ExecuteDelete();
            db.SaveChanges();
        }

        public void RestoreTrash(int id)
        {
            db.Goods.Where(i => i.DeletedAt > 0)
                .When(id > 0, i => i.Id == id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.DeletedAt, 0));
        }

        public IOperationResult<GoodsEntity> GoodsAction(int id, Dictionary<string, byte> data)
        {
            var model = db.Goods.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<GoodsEntity>.Fail("商品不存在");
            }
            foreach (var item in data)
            {
                if (!Can(model, item.Key))
                {
                    continue;
                }
                switch (item.Key)
                {
                    case "is_new":
                        model.IsNew = item.Value;
                        break;
                    case "is_hot":
                        model.IsHot = item.Value;
                        break;
                    case "is_best":
                        model.IsBest = item.Value;
                        break;
                    default:
                        break;
                }
            }
            db.Goods.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public bool Can(GoodsEntity model, string action)
        {
            if (client.UserId == 0)
            {
                return false;
            }
            return userStore.IsRole(client.UserId, "administrator");
        }

        public IPage<GoodsListItem> Search(QueryForm form,
            int[] idItems,
            int category = 0, int brand = 0)
        {
            if (!string.IsNullOrWhiteSpace(form.Keywords) && category < 1 && brand < 1)
            {
                var product = db.Products.Where(i => i.SeriesNumber == form.Keywords)
                    .FirstOrDefault();
                if (product is not null)
                {
                    var goods = AsList(db.Goods.Where(i => i.Id == product.GoodsId)).SingleOrDefault();
                    goods!.Products = [product];
                    return new Page<GoodsListItem>(1, form) 
                    { 
                        Items = [goods]
                    };
                } else
                {
                    var goods = AsList(db.Goods.Where(i => i.SeriesNumber == form.Keywords))
                    .FirstOrDefault();
                    if (goods is not null)
                    {
                        goods.Products = db.Products.Where(i => i.GoodsId == goods.Id).ToArray();
                        return new Page<GoodsListItem>(1, form)
                        {
                            Items = [goods]
                        };
                    }
                }

            }
            var res = db.Goods.When(idItems.Length > 0, i => idItems.Contains(i.Id))
                .Search(form.Keywords, "name")
                .When(category > 0, i => i.CatId == category)
                .When(brand > 0, i => i.BrandId == brand)
                .ToPage(form, AsList);
            foreach (var item in res.Items)
            {
                item.Products = db.Products.Where(i => i.GoodsId == item.Id).ToArray();
            }
            return res;
        }

        public AttributeResult AttributeList(int group_id, int goods_id = 0)
        {
            var res = new AttributeResult
            {
                ProductList = db.Products.Where(i => i.GoodsId == goods_id).OrderBy(i => i.Id).ToArray(),
                AttrList = db.Attributes.Where(i => i.GroupId == goods_id)
                    .OrderBy(i => i.Position).OrderBy(i => i.Type)
                    .ToArray().Select(i => new AttributeResult.AttributeFormatted(i)).ToArray()
            };
            foreach (var item in res.AttrList)
            {
                item.AttrItems = db.GoodsAttributes.Where(i => i.GoodsId == goods_id && i.AttributeId == item.Id).ToArray();
            }
            return res;
        }

        public IOperationResult<GoodsEntity> ImportJson(GoodsRawForm data)
        {
            if (!string.IsNullOrWhiteSpace(data.Sn) 
                && HasSeriesNumber(data.Sn))
            {
                return OperationResult<GoodsEntity>.Fail("商品已存在");
            }
            var model = new GoodsEntity()
            {
                CatId = new CategoryRepository(db, client).FindOrNew(data.Category),
                BrandId = new BrandRepository(db, client).FindOrNew(data.Brand),
                Name = data.Title,
                SeriesNumber = string.IsNullOrWhiteSpace(data.Sn) ? GenerateSn() : data.Sn,
                Thumb = data.Thumb,
                Picture = data.Thumb,
                Keywords = data.Keywords,
                Description = data.Description,
                Brief = data.Description,
                Content = data.Content,
                Price = data.Price,
                MarketPrice = data.Price,
                Stock = 1,
                Status = GoodsStatus.STATUS_SALE,
            };
            db.Goods.Save(model, client.Now);
            db.SaveChanges();
            if (data.Images?.Length is null or 0)
            {
                return OperationResult.Ok(model);
            }
            foreach (var item in data.Images)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                db.GoodsGalleries.Add(new GoodsGalleryEntity()
                {
                    GoodsId = model.Id,
                    Thumb = item,
                    File = item,
                    Type = GoodsStatus.FILE_TYPE_IMAGE
                });
            }
            db.SaveChanges();
            return OperationResult.Ok(model);
        }


        public bool HasSeriesNumber(string sn)
        {
            return db.Goods.Where(i => i.SeriesNumber == sn).Any();
        }

        public string GenerateSn()
        {
            string sn;
            var i = 0;
            var rand = new Random();
            do
            {
                i++;
                sn = $"SN{StrHelper.RandomNumber(rand, 8):D8}";
                if (!HasSeriesNumber(sn))
                {
                    break;
                }
            }
            while (i < 10);
            return sn;
        }

        public IPage<GoodsCardEntity> CardList(int goods, QueryForm form)
        {
            return db.GoodsCards.Where(i => i.GoodsId == goods)
                .Search(form.Keywords, "card_no")
                .OrderBy(i => i.OrderId)
                .OrderByDescending(i => i.Id).ToPage(form);
        }

        public void CardGenerate(int goods, int amount = 1)
        {
            GenerateCard(goods, amount);
            RefreshCardStock(goods);
        }

        public void CardRemove(int id)
        {
            var model = db.GoodsCards.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return;
            }
            db.GoodsCards.Remove(model);
            RefreshCardStock(model.GoodsId);
        }
        public void GenerateCard(int goods, int amount)
        {
            var prefix = DateTime.Now.ToString("yyyyMMddHHiiss");
            for (; amount >= 0; amount--)
            {
                db.GoodsCards.Add(new()
                {
                    GoodsId = goods,
                    CardNo = $"{prefix}{amount:D6}",
                    CardPwd = StrHelper.Random(20)
                });
            }
            db.SaveChanges();
        }

        public void RefreshCardStock(int goods)
        {
            var count = db.GoodsCards.Where(i => i.GoodsId == goods && i.OrderId < 1)
                .Count();
            db.Goods.Where(i => i.Id == goods).ExecuteUpdate(setters => 
                setters.SetProperty(i => i.Stock, count));
        }

        public IOperationResult<GoodsModel> Preview(int id)
        {
            var model = db.Goods.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<GoodsModel>("数据错误");
            }
            var res = model.CopyTo<GoodsModel>();
            res.Properties = [];
            res.StaticProperties = [];
            res.Category = db.Categories.Where(i => i.Id == model.CatId).SingleOrDefault();
            res.Brand = db.Brands.Where(i => i.Id == model.BrandId).SingleOrDefault();
            res.Products = db.Products.Where(i => i.GoodsId == model.Id).ToArray();
            res.Gallery = db.GoodsGalleries.Where(i => i.GoodsId == model.Id).ToArray();
            return OperationResult.Ok(res);
        }

        public void CardImport(int goods)
        {

        }

        public void CardExport(int goods)
        {

        }

        /**
         * 整理商品id
         * @throws Exception
         */
        public void SortOut()
        {
            db.Goods.RefreshPk((old_id, new_id) => {
                db.GoodsAttributes.Where(i => i.GoodsId == old_id).ExecuteUpdate(setters => setters.SetProperty(i => i.GoodsId, new_id));
                db.GoodsMetas.Where(i => i.GoodsId == old_id).ExecuteUpdate(setters => setters.SetProperty(i => i.GoodsId, new_id));
                db.GoodsGalleries.Where(i => i.GoodsId == old_id).ExecuteUpdate(setters => setters.SetProperty(i => i.GoodsId, new_id));
                db.GoodsCards.Where(i => i.GoodsId == old_id).ExecuteUpdate(setters => setters.SetProperty(i => i.GoodsId, new_id));
                db.GoodsIssues.Where(i => i.GoodsId == old_id).ExecuteUpdate(setters => setters.SetProperty(i => i.GoodsId, new_id));
                db.Products.Where(i => i.GoodsId == old_id).ExecuteUpdate(setters => setters.SetProperty(i => i.GoodsId, new_id));
                db.Carts.Where(i => i.GoodsId == old_id).ExecuteUpdate(setters => setters.SetProperty(i => i.GoodsId, new_id));
                db.OrderGoods.Where(i => i.GoodsId == old_id).ExecuteUpdate(setters => setters.SetProperty(i => i.GoodsId, new_id));
                db.Collects.Where(i => i.GoodsId == old_id).ExecuteUpdate(setters => setters.SetProperty(i => i.GoodsId, new_id));
                db.DeliveryGoods.Where(i => i.GoodsId == old_id).ExecuteUpdate(setters => setters.SetProperty(i => i.GoodsId, new_id));
                db.Comments.Where(i => i.ItemType == 0 && i.ItemId == old_id)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.ItemId, new_id));
            });
        }

        public IOperationResult<GoodsEntity> CrawlSave(GoodsForm data)
        {
            return Save(data);
        }

        public static IQueryable<GoodsListItem> AsList(IQueryable<GoodsEntity> query)
        {
            return query.Select(i => new GoodsListItem()
            {
                Id = i.Id,
                CatId = i.CatId,
                BrandId = i.BrandId,
                Name = i.Name,
                SeriesNumber = i.SeriesNumber,
                Thumb = i.Thumb,
                Description = i.Description,
                Price = i.Price,
                MarketPrice = i.MarketPrice,
                Stock = i.Stock,
                Sales = i.Sales,
                IsBest = i.IsBest,
                IsHot = i.IsHot,
                IsNew = i.IsNew,
                Status = i.Status,
                Type = i.Type,
                Position = i.Position,
                UpdatedAt = i.UpdatedAt,
                CreatedAt = i.CreatedAt,
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
            var data = db.Goods.Where(i => idItems.Contains(i.Id))
                .Select(i => new GoodsLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Thumb = i.Thumb,
                    SeriesNumber = i.SeriesNumber,
                })
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