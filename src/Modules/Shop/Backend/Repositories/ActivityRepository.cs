using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class ActivityRepository(ShopContext db, IClientContext client)
    {
        public IPage<ActivityListItem> GetList(int type, QueryForm form) 
        {
            var res = db.Activities.Search(form.Keywords, "name")
                .Where(i => i.Type == type)
                .OrderByDescending(i => i.Id)
                .ToPage<ActivityEntity, ActivityListItem>(form);
            if (type is ActivityType.TYPE_AUCTION or  ActivityType.TYPE_PRE_SALE or ActivityType.TYPE_BARGAIN)
            {
                IncludeGoods(res.Items);
            }
            return res;
        }

        private void IncludeGoods(ActivityListItem[] items)
        {
            var idItems = items.Select(item => int.Parse(item.Scope)).Where(i => i > 0)
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
                if (data.TryGetValue(int.Parse(item.Scope), out var res))
                {
                    item.Goods = res;
                }
            }
        }

        public IOperationResult<ActivityModel> Get(int type,int id) 
        {
            var model = db.Activities.Where(i => i.Type == type && i.Id == id)
                .SingleOrDefault();
            if (model is null) {
                return OperationResult<ActivityModel>.Fail("数据有误");
            }
            var res = model.CopyTo<ActivityModel>();
            if (type == ActivityType.TYPE_MIX) 
            {
                res.Configure = JsonSerializer.Deserialize<MixConfigure>(model.Configure);
            }
            if (type == ActivityType.TYPE_LOTTERY) {
                res.Configure = JsonSerializer.Deserialize<LotteryConfigure>(model.Configure);
            }
            return OperationResult.Ok(res);
        }

        public IOperationResult<ActivityEntity> Save(int type, ActivityForm data) 
        {
            var model = data.Id > 0 ? db.Activities.Where(i => i.Id == data.Id && i.Type == type).SingleOrDefault()
                : new ActivityEntity();
            if (model is null)
            {
                return OperationResult<ActivityEntity>.Fail("数据有误");
            }
            if (data.ScopeType < 1) {
                data.Scope = string.Empty;
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Thumb = data.Thumb;
            model.ScopeType = data.ScopeType;
            model.Scope = data.Scope;
            model.StartAt = data.StartAt;
            model.EndAt = data.EndAt;
            model.Configure = data.Configure;
            model.Type = type;
            db.Activities.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int type,int id) 
        {
            db.Activities.Where(i => i.Id == id && i.Type == type).ExecuteDelete();
            db.SaveChanges();
        }

        public ActivityTimeEntity[] TimeList() 
        {
            return db.ActivityTimes.OrderBy(i => i.StartAt).ToArray();
        }

        public IOperationResult<ActivityTimeEntity> Time(int id) 
        {
            var model = db.ActivityTimes.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<ActivityTimeEntity>("数据错误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<ActivityTimeEntity> TimeSave(ActivityTimeForm data) 
        {
            var model = data.Id > 0 ? db.ActivityTimes.Where(i => i.Id == data.Id).SingleOrDefault()
                : new ActivityTimeEntity();
            if (model is null)
            {
                return OperationResult<ActivityTimeEntity>.Fail("数据有误");
            }
            model.Title = data.Title;
            model.StartAt = data.StartAt;
            model.EndAt = data.EndAt;
            db.ActivityTimes.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void TimeRemove(int id) 
        {
            db.ActivityTimes.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IPage<SecKillGoodsListItem> GoodsList(QueryForm form, 
            int activity, int time) 
        {
            var res = db.SecKillGoods.Where(i => i.ActId == activity && i.TimeId == time)
                .ToPage<SecKillGoodsEntity, SecKillGoodsListItem>(form);
            GoodsRepository.Include(db, res.Items);
            return res;
        }

        public IOperationResult<SecKillGoodsEntity> GoodsSave(
            SecKillGoodsForm data) 
        {
            SecKillGoodsEntity? model;
            if (data.Id > 0) {
                model = db.SecKillGoods.Where(i => i.Id == data.Id).SingleOrDefault();
            } 
            else 
            {
                model = db.SecKillGoods.Where(i => i.ActId == data.ActId 
                && i.GoodsId == data.GoodsId 
                && i.TimeId == data.TimeId)
                    .SingleOrDefault();
            }
            if (model is null) {
                model = new();
            }
            model.ActId = data.ActId;
            model.TimeId = data.TimeId;
            model.GoodsId = data.GoodsId;
            model.Price = data.Price;
            model.Amount = data.Amount;
            model.EveryAmount = data.EveryAmount;
            db.SecKillGoods.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void GoodsRemove(int id) 
        {
            db.SecKillGoods.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }


    }
}
