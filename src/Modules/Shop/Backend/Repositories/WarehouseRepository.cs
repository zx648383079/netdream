using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class WarehouseRepository(ShopContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<WarehouseEntity> GetList(QueryForm form)
        {
            return db.Warehouses.Search(form.Keywords, "name", "link_user", "tel", "address")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<WarehouseEntity> Get(int id)
        {
            var model = db.Warehouses.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<WarehouseEntity>.Fail("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<WarehouseModel> GetWithRegion(int id)
        {
            var model = db.Warehouses.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<WarehouseModel>.Fail("数据有误");
            }
            var res = model.CopyTo<WarehouseModel>();
            res.Region = GetRegion(id);
            return OperationResult.Ok(res);
        }

        public ListLabelItem[] GetRegion(int id)
        {
            var region = db.WarehouseRegions.Where(i => i.WarehouseId == id)
                .Pluck(i => i.RegionId);
            if (region.Length == 0)
            {
                return [];
            }
            return db.Regions.Where(i => region.Contains(i.Id))
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .ToArray();
        }

        public IOperationResult<WarehouseEntity> Save(WarehouseForm data)
        {
            var model = data.Id > 0 ? db.Warehouses.Where(i => i.Id == data.Id).SingleOrDefault() 
                : new WarehouseEntity();
            if (model is null)
            {
                return OperationResult<WarehouseEntity>.Fail("数据有误");
            }
            model.Name = data.Name;
            model.Tel = data.Tel;
            model.Address = data.Address;
            model.LinkUser = data.LinkUser;
            model.Remark = data.Remark;
            model.Latitude = data.Latitude;
            model.Longitude = data.Longitude;
            db.Warehouses.Save(model, client.Now);
            db.SaveChanges();
            if (data.Region?.Length is null or 0)
            {
                return OperationResult.Ok(model);
            }
            var (add, _, del) = ModelHelper.SplitId(data.Region,
                db.WarehouseRegions.Where(i => i.WarehouseId == model.Id)
                .Pluck(i => i.RegionId));
            if (del.Count > 0)
            {
                db.WarehouseRegions.Where(i => i.WarehouseId == model.Id
                    && del.Contains(i.RegionId)).ExecuteDelete();
            }
            if (add.Count > 0)
            {
                foreach (var item in add)
                {
                    db.WarehouseRegions.Add(new WarehouseRegionEntity()
                    {
                        RegionId = item,
                        WarehouseId = model.Id
                    });
                }
            }
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Warehouses.Where(i => i.Id == id).ExecuteDelete();
            db.WarehouseGoods.Where(i => i.WarehouseId == id).ExecuteDelete();
            db.WarehouseRegions.Where(i => i.WarehouseId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IPage<WarehouseGoodsListItem> GoodsList(QueryForm form, int warehouse = 0, int goods = 0, int product = 0)
        {
            var goodIds = Array.Empty<int>();
            if (!string.IsNullOrWhiteSpace(form.Keywords))
            {
                goodIds = db.Goods.Search(form.Keywords, "name").Pluck(i => i.Id);
                if (goodIds.Length == 0)
                {
                    return new Page<WarehouseGoodsListItem>();
                }
            }
            var res = db.WarehouseGoods.When(goodIds.Length > 0, i => goodIds.Contains(i.GoodsId))
                .When(warehouse > 0, i => i.WarehouseId == warehouse)
                .When(goods > 0, i => i.GoodsId == goods)
                .When(product > 0, i => i.ProductId == product)
                .ToPage<WarehouseGoodsEntity, WarehouseGoodsListItem>(form);
            Include(res.Items);
            GoodsRepository.Include(db, res.Items);
            ProductRepository.Include(db, res.Items);
            return res;
        }

        
        public IPage<WarehouseLogListItem> LogList(QueryForm form, int warehouse = 0, int goods = 0, int product = 0)
        {
            var res = db.WarehouseLogs
                .Search(form.Keywords, "remark")
                .When(warehouse > 0, i => i.WarehouseId == warehouse)
                .When(goods > 0, i => i.GoodsId == goods)
                .When(product > 0, i => i.ProductId == product)
                .OrderByDescending(i => i.Id)
                .ToPage<WarehouseLogEntity, WarehouseLogListItem>(form);
            Include(res.Items);
            GoodsRepository.Include(db, res.Items);
            userStore.Include(res.Items);
            return res;
        }

        public IOperationResult<WarehouseLogEntity> GoodsChange(WarehouseLogForm data)
        {
            if (data.Amount <= 0)
            {
                return OperationResult<WarehouseLogEntity>.Fail("请输入不为0 的数量");
            }
            var model = db.WarehouseGoods
                .Where(i => i.WarehouseId == data.WarehouseId 
                    && i.GoodsId == data.GoodsId
                    && i.ProductId == data.ProductId)
                .FirstOrDefault();
            if (model is null)
            {
                model = new WarehouseGoodsEntity()
                {
                    WarehouseId = data.WarehouseId,
                    GoodsId = data.GoodsId,
                    ProductId = data.ProductId,
                    Amount = data.Amount,
                };
            }
            else
            {
                model.Amount += data.Amount;
            }
            var log = new WarehouseLogEntity()
            {
                WarehouseId = data.WarehouseId,
                GoodsId = data.GoodsId,
                ProductId = data.ProductId,
                Amount = data.Amount,
                UserId = client.UserId,
                Remark = data.Remark,
            };
            db.WarehouseGoods.Save(model);
            db.WarehouseLogs.Save(log, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(log);
        }

        private void Include(IWithWarehouseModel[] items)
        {
            var idItems = items.Select(item => item.WarehouseId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Warehouses.Where(i => idItems.Contains(i.Id))
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.WarehouseId > 0 && data.TryGetValue(item.WarehouseId, out var res))
                {
                    item.Warehouse = res;
                }
            }
        }

    }
}
