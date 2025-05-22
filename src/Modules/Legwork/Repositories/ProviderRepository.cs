using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Legwork.Entities;
using NetDream.Modules.Legwork.Forms;
using NetDream.Modules.Legwork.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Legwork.Repositories
{
    public class ProviderRepository(LegworkContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<ProviderEntity> GetList(QueryForm form)
        {
            return db.Provider.Search(form.Keywords, "name").ToPage(form);
        }

        public IOperationResult<ProviderEntity> Get(int id)
        {
            var model = db.Provider.Where(i => i.Id == id)
                .FirstOrDefault();
            if (model is null)
            {
                return OperationResult<ProviderEntity>.Fail("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public ProviderEntity GetSelf()
        {
            var model = db.Provider.Where(i => i.UserId == client.UserId).FirstOrDefault();
            if (model is not null)
            {
                return model;
            }
            return new ProviderEntity()
            {
                UserId = client.UserId
            };
        }

        public void ApplyCategory(int[] idItems)
        {
            var exist = db.CategoryProvider.Where(i => i.UserId == client.UserId)
                .Select(i => i.CatId).ToArray();
            var add = ModelHelper.Diff(idItems, exist);
            var update = ModelHelper.Diff(exist, idItems);
            if (add.Length > 0)
            {
                foreach (var item in add)
                {
                    db.CategoryProvider.Add(new()
                    {
                        UserId = client.UserId,
                        Status = 0,
                        CatId = item
                    });
                }
            }
            if (update.Length > 0)
            {
                db.CategoryProvider.Where(i =>
                    i.UserId == client.UserId && update.Contains(i.CatId)
                    && i.Status != LegworkRepository.STATUS_DISALLOW)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 0));
            }
            db.SaveChanges();
        }

        public IOperationResult<ProviderEntity> Change(int id, byte status)
        {
            var model = db.Provider.Where(i => i.Id == id)
                .FirstOrDefault();
            if (model is null)
            {
                return OperationResult<ProviderEntity>.Fail("数据有误");
            }
            model.Status = status;
            db.Provider.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void ChangeCategory(int id, int[] category, byte status)
        {
            db.CategoryProvider.Where(i => i.UserId == id && category.Contains(i.CatId))
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, status));
            db.SaveChanges();
        }

        public IOperationResult<ProviderEntity> Save(ProviderForm data)
        {
            var model = GetSelf();
            model.Tel = data.Tel;
            model.Name = data.Name;
            model.Address = data.Address;
            model.Logo = data.Logo;
            model.UserId = client.UserId;
            model.Status = 0;
            db.Provider.Save(model, client.Now);
            if (data.Categories?.Length > 0)
            {
                ApplyCategory(data.Categories.Select(i => 
                i.GetValueKind() == System.Text.Json.JsonValueKind.Object
                && i.AsObject().TryGetPropertyValue("id", out var r) ?
                r.GetValue<int>() : i.GetValue<int>()).ToArray());
            }
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Provider.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IPage<OrderListItem> OrderList(QueryForm form, int status = 0, int id = 0, int user_id = 0, int waiter_id = 0)
        {
            var serviceId = Array.Empty<int>();
            if (string.IsNullOrWhiteSpace(form.Keywords))
            {
                serviceId = db.Service.Search(form.Keywords, "name")
                    .Where(i => i.Status == LegworkRepository.STATUS_ALLOW)
                    .Select(i => i.Id).ToArray();
                if (serviceId.Length == 0)
                {
                    return new Page<OrderListItem>(0, form);
                }
            }
            var res = db.Order.Where(i => i.ProviderId == client.UserId)
                .When(status > 0, i => i.Status == status,  
                i => i.Status >= OrderRepository.STATUS_UN_PAY)
                   .When(serviceId.Length > 0, i => serviceId.Contains(i.ServiceId))
                .When(id > 0, i => i.Id == id)
                .When(user_id > 0, i => i.UserId == user_id)
                .When(waiter_id > 0, i => i.WaiterId == waiter_id)
                .OrderByDescending(i => i.Id)
                .ToPage(form).CopyTo<OrderEntity, OrderListItem>();
            ServiceRepository.Include(db, res.Items);
            OrderRepository.IncludeUser(db, userStore, res.Items);
            return res;
        }

        public IOperationResult<OrderEntity> Order(int id)
        {
            var model = db.Order.Where(i => i.Id == id && i.ProviderId == client.UserId)
                .FirstOrDefault();
            if (model is null)
            {
                return OperationResult<OrderEntity>.Fail("订单不存在");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<OrderEntity> AssignOrder(int id, int waiter_id)
        {
            var model = db.Order.Where(i => i.Id == id && i.ProviderId == client.UserId)
                .FirstOrDefault();
            if (model is null)
            {
                return OperationResult<OrderEntity>.Fail("订单不存在");
            }
            if (WaiterRepository.IsWaiter(db, waiter_id))
            {
                return OperationResult<OrderEntity>.Fail("不能指定此用户");
            }
            if (!ServiceRepository.IsAllowWaiter(db, model.ServiceId, waiter_id))
            {
                return OperationResult<OrderEntity>.Fail("不能指定此用户");
            }
            if (model.Status != OrderRepository.STATUS_PAID_UN_TAKING)
            {
                return OperationResult<OrderEntity>.Fail("订单状态错误");
            }
            model.WaiterId = waiter_id;
            new OrderRepository(db, client, null, null).ChangeStatus(model, OrderRepository.STATUS_TAKEN);
            return OperationResult.Ok(model);
        }

        public bool HasService(int id)
        {
            return db.Service.Where(i => i.Id == id && i.UserId == client.UserId).Any();
        }

        public IPage<CategoryListItem> CategoryList(QueryForm form, 
            int category = 0, int status = 0, 
            bool all = false, int user_id = 0)
        {
            if (user_id == 0)
            {
                user_id = client.UserId;
            }
            var links = db.CategoryProvider
                .When(category > 0,  i => i.CatId == category)
                .When(status > 0, i => i.Status == 1)
                .Where(i => i.UserId == user_id)
                .Select(i => new KeyValuePair<int, byte>(i.CatId, i.Status)).ToDictionary();
            var res = db.Categories.Search(form.Keywords, "name")
                .When(!all, i => links.Keys.Contains(i.Id))
                .ToPage(form).CopyTo<CategoryEntity, CategoryListItem>();
            foreach (var item in res.Items)
            {
                if (links.TryGetValue(item.Id, out var s))
                {
                    item.Status = s;
                }
            }
            return res;
        }
    }
}
