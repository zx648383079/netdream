using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.UserProfile;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class ShippingRepository(ShopContext db, 
        IClientContext client,
        ProfileContext regionStore)
    {
        public IPage<ShippingEntity> GetList(QueryForm form)
        {
            return db.Shipping.Search(form.Keywords, "name").ToPage(form);
        }

        public IOperationResult<ShippingModel> Get(int id, bool full = false)
        {
            var model = db.Shipping.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<ShippingModel>("数据错误");
            }
            var res = model.CopyTo<ShippingModel>();
            if (!full)
            {
                return OperationResult.Ok(res);
            }
            res.Groups = db.ShippingGroups.Where(i => i.ShippingId == model.Id)
                .ToArray().CopyTo<ShippingGroupEntity, ShippingGroupModel>();
            Include(res.Groups);
            return OperationResult.Ok(res);
        }

        private void Include(ShippingGroupModel[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var idItems = items.Select(i => i.Id).ToArray();
            var linkItems = db.ShippingRegions.Where(i => idItems.Contains(i.GroupId))
                .ToArray();
            if (linkItems.Length == 0)
            {
                return;
            }
            var linkId = linkItems.Select(i => i.RegionId).ToArray();
            var data = regionStore.Regions.Where(i => linkId.Contains(i.Id)).Select(i => new ListLabelItem(i.Id, i.Name))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                item.Regions = linkItems.Where(i => i.GroupId == item.Id)
                    .Select(i => data[i.RegionId]).ToArray();
            }
        }

        public IOperationResult<ShippingEntity> Save(ShippingForm data)
        {
            var model = data.Id > 0 ? db.Shipping.Where(i => i.Id == data.Id).SingleOrDefault()
                : new ShippingEntity();
            if (model is null)
            {
                return OperationResult.Fail<ShippingEntity>("数据错误");
            }
            model.Name = data.Name;
            model.Position = data.Position;
            model.Description = data.Description;
            model.Code = data.Code;
            model.Icon = data.Icon;
            model.Method = data.Method;
            db.Shipping.Save(model, client.Now);
            db.SaveChanges();
            if (data.Groups?.Length is null or 0)
            {
                return OperationResult.Ok(model);
            }
            foreach (var item in data.Groups)
            {
                item.ShippingId = model.Id;
                SaveGroup(item);
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult SaveGroup(ShippingGroupForm data)
        {
            var model = data.Id > 0 ? db.ShippingGroups.Where(i => i.Id == data.Id).SingleOrDefault()
                : new ShippingGroupEntity();
            if (model is null)
            {
                return OperationResult.Fail("数据错误");
            }
            model.FreeStep = data.FreeStep;
            model.FirstStep = data.FirstStep;
            model.FirstFee = data.FirstFee;
            model.AdditionalFee = data.AdditionalFee;
            model.Additional = data.Additional;
            model.IsAll = data.IsAll;
            model.ShippingId = data.ShippingId;
            db.ShippingGroups.Save(model);
            db.SaveChanges();
            SaveRegion(model.IsAll > 0 ? [] : data.Regions.Select(i => i.Id).ToArray(), model.Id, model.ShippingId);
            return OperationResult.Ok();
        }

        public void SaveRegion(int[] items, int group_id, int shipping_id)
        {
            var exist = db.ShippingRegions.Where(
                i => i.GroupId == group_id && i.ShippingId == shipping_id)
                .Pluck(i => i.RegionId);
            var add = ModelHelper.Diff(items, exist);
            var remove = ModelHelper.Diff(exist, items);
            if (add.Length > 0)
            {
                foreach (var item in add)
                {
                    db.ShippingRegions.Add(new ShippingRegionEntity()
                    {
                        ShippingId = shipping_id,
                        GroupId = group_id,
                        RegionId = item
                    });
                }
            }
            if (remove.Length > 0)
            {
                db.ShippingRegions.Where(
                i => i.GroupId == group_id && i.ShippingId == shipping_id
                && remove.Contains(i.RegionId)).ExecuteDelete();
            }
            db.SaveChanges();
        }

        public void Remove(int id)
        {
            db.Shipping.Where(i => i.Id == id).ExecuteDelete();
            db.ShippingGroups.Where(i => i.ShippingId == id).ExecuteDelete();
            db.ShippingRegions.Where(i => i.ShippingId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public ListLabelItem[] All()
        {
            return db.Shipping.Select(i => new ListLabelItem(i.Id, i.Name)).ToArray();
        }


    }
}
