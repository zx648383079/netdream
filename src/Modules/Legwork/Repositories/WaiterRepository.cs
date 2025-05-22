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
    internal class WaiterRepository(LegworkContext db, IClientContext client)
    {
        public IPage<WaiterEntity> GetList(QueryForm form)
        {
            return db.Waiter.Search(form.Keywords, "name").ToPage(form);
        }

        public IOperationResult<WaiterEntity> Get(int id)
        {
            var model = db.Waiter.Where(i => i.Id == id).FirstOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<WaiterEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public WaiterEntity GetSelf()
        {
            var model = db.Waiter.Where(i => i.UserId == client.UserId).FirstOrDefault();
            if (model is not null)
            {
                return model;
            }
            return new WaiterEntity()
            {
                UserId = client.UserId,
            };
        }

        public IOperationResult ApplyService(int[] idItems)
        {
            if (!IsWaiter(db, client.UserId))
            {
                return OperationResult.Fail("您的身份还在审核中。。。");
            }
            var exist = db.ServiceWaiter.Where(i => i.UserId == client.UserId)
                .Select(i => i.ServiceId).ToArray();
            var add = ModelHelper.Diff(idItems, exist);
            var update = ModelHelper.Diff(exist, idItems);
            if (add.Length > 0)
            {
                foreach (var item in add)
                {
                    db.ServiceWaiter.Add(new()
                    {
                        UserId = client.UserId,
                        Status = 0,
                        ServiceId = item
                    });
                }
            }
            if (update.Length > 0)
            {
                db.ServiceWaiter.Where(i =>
                    i.UserId == client.UserId && update.Contains(i.ServiceId)
                    && i.Status != LegworkRepository.STATUS_DISALLOW)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 0));
            }
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult<WaiterEntity> Change(int id, byte status)
        {
            var model = db.Waiter.Where(i => i.Id == id)
                .FirstOrDefault();
            if (model is null)
            {
                return OperationResult<WaiterEntity>.Fail("数据有误");
            }
            model.Status = status;
            db.Waiter.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<WaiterEntity> Save(WaiterForm data)
        {
            var model = GetSelf();
            model.Tel = data.Tel;
            model.Name = data.Name;
            model.Address = data.Address;
            model.UserId = client.UserId;
            model.Status = 0;
            db.Waiter.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Waiter.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }


        public IPage<ServiceListItem> ServiceList(QueryForm form, 
            int category = 0, bool all = false)
        {
            var links = db.ServiceWaiter.Where(i => i.UserId == client.UserId)
                .Select(i => new KeyValuePair<int, byte>(i.ServiceId, i.Status))
                .ToDictionary();
            var res = db.Service.Search(form.Keywords, "name")
                .When(category > 0, i => i.CatId == category)
                .When(!all, i => links.Keys.Contains(i.Id))
                .Where(i => i.Status == LegworkRepository.STATUS_ALLOW)
                .Select(i => new ServiceListItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Brief = i.Brief,
                    Price = i.Price,
                    CatId = i.CatId,
                    UserId = i.UserId,
                })
                .ToPage(form);
            foreach (var item in res.Items)
            {
                item.Status = links.TryGetValue(item.Id, out var s) ? s : -1;
            }
            return res;
        }

        public IPage<OrderListItem> OrderList(QueryForm form, byte status = 0)
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
            var res = db.Order.Where(i => i.WaiterId == client.UserId)
                .When(serviceId.Length > 0, i => serviceId.Contains(i.ServiceId))
            .When(status > 0,
                i => i.Status == status,
                i => i.Status >= OrderRepository.STATUS_UN_PAY)
                .OrderByDescending(i => i.Id).ToPage(form)
                .CopyTo<OrderEntity, OrderListItem>();
            ServiceRepository.Include(db, res.Items);
            return res;
        }

        public IOperationResult<OrderEntity> Taking(int id)
        {
            var order = db.Order.Where(i => i.WaiterId == 0 
                && i.Status == OrderRepository.STATUS_PAID_UN_TAKING
                && i.Id == id).FirstOrDefault();
            if (order is null)
            {
                return OperationResult<OrderEntity>.Fail("单已失效");
            }
            if (!ServiceRepository.IsAllowWaiter(db, order.ServiceId, client.UserId))
            {
                return OperationResult<OrderEntity>.Fail("没有接单权限");
            }
            order.WaiterId = client.UserId;
            new OrderRepository(db, client, null, null)
                .ChangeStatus(order, OrderRepository.STATUS_TAKING_UN_DO);
            return OperationResult.Ok(order);
        }

        public IOperationResult<OrderEntity> Taken(int id)
        {
            var order = db.Order.Where(i => i.WaiterId == client.UserId
                && i.Status == OrderRepository.STATUS_TAKING_UN_DO
                && i.Id == id).FirstOrDefault();
            if (order is null)
            {
                return OperationResult<OrderEntity>.Fail("单已失效");
            }
            new OrderRepository(db, client, null, null)
                .ChangeStatus(order, OrderRepository.STATUS_TAKEN);
            return OperationResult.Ok(order);
        }

        public IPage<OrderListItem> WaitTakingList(QueryForm form)
        {
            var serviceId = db.ServiceWaiter
                .Where(i => i.UserId == client.UserId 
                    && i.Status == LegworkRepository.STATUS_ALLOW)
                .Select(i => i.ServiceId).ToArray();
            if (serviceId.Length > 0 && !string.IsNullOrWhiteSpace(form.Keywords))
            {
                serviceId = db.Service.Search(form.Keywords, "name")
                    .Where(i => i.Status == LegworkRepository.STATUS_ALLOW 
                        && serviceId.Contains(i.Id))
                    .Select(i => i.Id).ToArray();
            }
            if (serviceId.Length == 0)
            {
                return new Page<OrderListItem>();
            }
            var res = db.Order.Where(i => i.WaiterId == 0 
                && serviceId.Contains(i.ServiceId)
                && i.Status == OrderRepository.STATUS_PAID_UN_TAKING)
                .OrderByDescending(i => i.Id)
                .Select(i => new OrderListItem()
                {
                    Id = i.Id,
                    UserId = i.UserId,
                    ServiceId = i.ServiceId,
                    OrderAmount = i.OrderAmount,
                    Amount = i.Amount,
                    Status = i.Status,
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt
                })
                .ToPage(form);
            ServiceRepository.Include(db, res.Items);
            return res;
        }
        internal static bool IsWaiter(LegworkContext db, int user)
        {
            return db.Waiter.Where(i => i.UserId == user && i.Status == LegworkRepository.STATUS_ALLOW)
                .Any();
        }
    }
}