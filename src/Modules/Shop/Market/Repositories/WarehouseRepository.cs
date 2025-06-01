using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class WarehouseRepository(ShopContext db)
    {
        public IOperationResult<WarehouseModel> GetByRegion(
            int regionId, 
            int goodsId, 
            int productId = 0)
        {
            if (regionId < 1)
            {
                return OperationResult<WarehouseModel>.Fail("地址不能为空");
            }
            var regionIds = new RegionRepository(db).GetPathId(regionId);
            var idItems = db.WarehouseRegions.Where(i => regionIds.Contains(i.RegionId))
                .Pluck(i => i.WarehouseId);
            if (idItems.Length == 0)
            {
                return OperationResult<WarehouseModel>.Fail("所选地址没有仓库");
            }
            var goods = db.WarehouseGoods.Where(i => 
                idItems.Contains(i.WarehouseId) && i.GoodsId == goodsId 
                && i.ProductId == productId)
                .OrderByDescending(i => i.Amount)
                .FirstOrDefault();
            if (goods is null)
            {
                return OperationResult<WarehouseModel>.Fail("所选地址没有库存");
            }
            var model = db.Warehouses.Where(i => i.Id == goods.WarehouseId)
                .SingleOrDefault();
            if (model is null)
            {
                return OperationResult<WarehouseModel>.Fail("所选地址没有库存");
            }
            var res = model.CopyTo<WarehouseModel>();
            res.Stock = goods.Amount;
            return OperationResult.Ok(res);
        }

        public int GetStock(int regionId, int goodsId, int productId = 0)
        {
            if (regionId < 1)
            {
                return 0;
            }
            var regionIds = new RegionRepository(db).GetPathId(regionId);
            var idItems = db.WarehouseRegions.Where(i => regionIds.Contains(i.RegionId))
                .Pluck(i => i.WarehouseId);
            if (idItems.Length == 0)
            {
                return 0;
            }
            return db.WarehouseGoods.Where(i =>
                idItems.Contains(i.WarehouseId) && i.GoodsId == goodsId
                && i.ProductId == productId)
                .OrderByDescending(i => i.Amount)
                .Value(i => i.Amount);
        }

        public bool UpdateStock(int regionId, int goodsId, int productId, int amount)
        {
            if (regionId < 1)
            {
                return false;
            }
            var regionIds = new RegionRepository(db).GetPathId(regionId);
            var idItems = db.WarehouseRegions.Where(i => regionIds.Contains(i.RegionId))
                .Pluck(i => i.WarehouseId);
            if (idItems.Length == 0)
            {
                return false;
            }
            db.WarehouseGoods.Where(i =>
                idItems.Contains(i.WarehouseId) && i.GoodsId == goodsId
                && i.ProductId == productId)
                .OrderByDescending(i => i.Amount)
                .Take(1)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Amount, amount));
            db.SaveChanges();
            return true;
        }

    }
}
