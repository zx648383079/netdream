using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class AttributeRepository(ShopContext db, IClientContext client)
    {
        public IPage<AttributeListItem> GetList(int group, QueryForm form)
        {
            var res = db.Attributes.Search(form.Keywords, "name")
                .Where(i => i.GroupId == group)
                .OrderByDescending(i => i.Id)
                .ToPage<AttributeEntity, AttributeListItem>(form);
            IncludeGroup(res.Items);
            return res;
        }

        private void IncludeGroup(AttributeListItem[] items)
        {
            var idItems = items.Select(item => item.GroupId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.AttributeGroups.Where(i => idItems.Contains(i.Id))
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.GroupId > 0 && data.TryGetValue(item.GroupId, out var res))
                {
                    item.Group = res;
                }
            }
        }

        public IOperationResult<AttributeEntity> Get(int id)
        {
            var model = db.Attributes.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<AttributeEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<AttributeEntity> Save(AttributeForm data)
        {
            var model = data.Id > 0 ? db.Attributes.Where(i => i.Id == data.Id).SingleOrDefault()
                : new AttributeEntity();
            if (model == null)
            {
                return OperationResult.Fail<AttributeEntity>("数据有误");
            }
            model.Name = data.Name;
            model.PropertyGroup = data.PropertyGroup;
            model.DefaultValue = data.DefaultValue;
            model.Position = data.Position;
            model.GroupId = data.GroupId;
            model.InputType = data.InputType;
            model.SearchType = data.SearchType;
            model.Type = data.Type;
            db.Attributes.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Activities.Where(i => i.Id == id).ExecuteDelete();
            db.GoodsAttributes.Where(i => i.AttributeId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IPage<AttributeGroupEntity> GroupList(QueryForm form)
        {
            return db.AttributeGroups.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<AttributeGroupEntity> GroupGet(int id)
        {
            var model = db.AttributeGroups.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<AttributeGroupEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<AttributeGroupEntity> GroupSave(AttributeGroupForm data)
        {
            var model = data.Id > 0 ? db.AttributeGroups.Where(i => i.Id == data.Id).SingleOrDefault()
                : new AttributeGroupEntity();
            if (model == null)
            {
                return OperationResult.Fail<AttributeGroupEntity>("数据有误");
            }
            model.Name = data.Name;
            model.PropertyGroups = data.PropertyGroups;
            db.AttributeGroups.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void GroupRemove(int id)
        {
            db.AttributeGroups.Where(i => i.Id == id).ExecuteDelete();
            var attrId = db.Attributes.Where(i => i.GroupId == id).Pluck(i => i.Id);
            if (attrId.Length > 0)
            {
                db.Attributes.Where(i => i.GroupId == id).ExecuteDelete();
                db.GoodsAttributes.Where(i => attrId.Contains(i.AttributeId)).ExecuteDelete();
            }
            var goodsId = db.Goods.Where(i => i.AttributeGroupId == id).Pluck(i => i.Id);
            if (goodsId.Length > 0)
            {
                db.Goods.Where(i => i.AttributeGroupId == id)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.AttributeGroupId, 0));
                db.Products.Where(i => goodsId.Contains(i.GoodsId)).ExecuteDelete();
            }
            db.SaveChanges();
        }

        public AttributeGroupModel[] GroupAll()
        {
            var items = db.AttributeGroups.ToArray();
            return items.Select(i => new AttributeGroupModel(i)).ToArray();
        }
    }
}
