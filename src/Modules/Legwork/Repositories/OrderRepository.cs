using MediatR;
using NetDream.Modules.Legwork.Entities;
using NetDream.Modules.Legwork.Forms;
using NetDream.Modules.Legwork.Models;
using NetDream.Modules.Trade.Forms;
using NetDream.Modules.Trade.Listeners;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetDream.Modules.Legwork.Repositories
{
    public class OrderRepository(LegworkContext db, 
        IClientContext client,
        IUserRepository userStore,
        IMediator mediator)
    {
        public const byte STATUS_CANCEL = 1;
        public const byte STATUS_INVALID = 2;
        public const byte STATUS_UN_PAY = 10;
        public const byte STATUS_PAYING = 12;
        public const byte STATUS_PAID_UN_TAKING = 20; // 已支付待接单
        public const byte STATUS_TAKING_UN_DO = 40; // 已接单待做
        public const byte STATUS_TAKEN = 60; // 已完成服务
        public const byte STATUS_FINISH = 80;    // 客户已评价
        public const byte STATUS_REFUNDED = 81;

        public IPage<OrderListItem> GetList(QueryForm form, int status = 0, int id = 0, int user_id = 0, int service_id = 0, int provider_id = 0, int waiter_id = 0)
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
            var res = db.Order.When(status > 0, 
                i => i.Status == status, 
                i => i.Status >= STATUS_UN_PAY)
                .When(serviceId.Length > 0, i => serviceId.Contains(i.ServiceId))
                .When(id > 0, i => i.Id == id)
                .When(user_id > 0, i => i.UserId == user_id)
                .When(service_id > 0, i => i.ServiceId == service_id)
                .When(waiter_id > 0, i => i.WaiterId == waiter_id)
                .When(provider_id > 0, i => i.ProviderId == provider_id)
                .OrderByDescending(i => i.Id)
                .ToPage(form).CopyTo<OrderEntity, OrderListItem>();
            ServiceRepository.Include(db, res.Items);
            IncludeUser(db, userStore, res.Items);
            return res;
        }


        public IPage<OrderListItem> GetSelfList(QueryForm form, int status = 0, int id = 0)
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
            var res = db.Order.When(status > 0,
                i => i.Status == status,
                i => i.Status >= STATUS_UN_PAY)
                .When(serviceId.Length > 0, i => serviceId.Contains(i.ServiceId))
                .When(id > 0, i => i.Id == id)
                .Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id)
                .ToPage(form).CopyTo<OrderEntity, OrderListItem>();
            ServiceRepository.Include(db, res.Items);
            IncludeUser(db, userStore, res.Items);
            return res;
        }

        public async Task<object> PayAsync(OrderEntity order)
        {
            var res = await mediator.Send(new PayRequest()
            {
                Payment = "wechat",
                SourceType = ModuleModelType.TYPE_LEGWORK,
                SourceId = order.Id,
                BuyerId = client.UserId,
                TotalAmount = order.OrderAmount,
                Subject = "代取件订单支付",
                ReferenceType = "web"
            });
            if (res.Status == PayOperateStatus.Success)
            {

            }
            return res;
        }

        public IOperationResult<OrderEntity> Comment(int id, byte rank = 10)
        {
            var order = db.Order.Where(i => i.UserId == client.UserId && i.Id == id)
                .FirstOrDefault();
            if (order is null)
            {
                return OperationResult<OrderEntity>.Fail("订单不存在");
            }
            if (order.Status != STATUS_TAKEN
                && order.Status != STATUS_TAKING_UN_DO
                && order.Status != STATUS_PAID_UN_TAKING)
            {
                return OperationResult<OrderEntity>.Fail("此订单无法评价");
            }
            order.ServiceScore = rank;
            ChangeStatus(order, STATUS_FINISH);
            return OperationResult.Ok(order);
        }

        public IOperationResult<OrderEntity> Cancel(int id)
        {
            var order = db.Order.Where(i => i.UserId == client.UserId && i.Id == id)
                .FirstOrDefault();
            if (order is null)
            {
                return OperationResult<OrderEntity>.Fail("订单不存在");
            }
            if (order.Status != STATUS_UN_PAY)
            {
                return OperationResult<OrderEntity>.Fail("此订单无法取消，可以联系店主");
            }
            ChangeStatus(order, STATUS_CANCEL);
            return OperationResult.Ok(order);
        }

        public IOperationResult<OrderEntity> Create(OrderForm data)
        {
            if (data.ServiceId < 1)
            {
                return OperationResult<OrderEntity>.Fail("选择服务");
            }
            var service = db.Service.Where(i => i.Id == data.ServiceId).FirstOrDefault();
            if (service is null)
            {
                return OperationResult<OrderEntity>.Fail("选择服务");
            }
            var form = JsonSerializer.Deserialize<ServiceFormItem[]>(service.Form);
            var remark = new List<ServiceFormValue>();
            foreach (var item in form)
            {
                if (!data.Remark.TryGetValue(item.Name, out var value) && item.Required > 0)
                {
                    return OperationResult<OrderEntity>.Fail($"{item.Label} 必填");
                }
                remark.Add(new ServiceFormValue()
                {
                    Name = item.Name,
                    Label = item.Label,
                    Only = item.Only,
                    Value = value ?? string.Empty
                });
            }
            var order = new OrderEntity();
            order.UserId = client.UserId;
            order.Status = STATUS_UN_PAY;
            order.ServiceId = service.Id;
            order.Remark = JsonSerializer.Serialize(remark);
            order.Amount = Math.Max(data.Amount, 1);
            order.OrderAmount = service.Price * order.Amount;
            db.Order.Save(order, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(order);
        }

        public void ChangeStatus(OrderEntity model, byte status)
        {
            var now = client.Now;
            model.Status = status;
            switch (status)
            {
                case STATUS_PAID_UN_TAKING:
                    model.PayAt = now;
                    break;
                case STATUS_TAKING_UN_DO:
                    model.TakingAt = now;
                    break;
                case STATUS_TAKEN:
                    model.TakenAt = now;
                    break;
                case STATUS_FINISH:
                    model.FinishAt = now;
                    break;
                default:
                    break;
            }
            db.Order.Save(model, now);
            db.OrderLog.Add(new OrderLogEntity()
            {
                OrderId = model.Id,
                UserId = client.UserId > 0 ? client.UserId : model.UserId,
                Status = status,
                CreatedAt = now,
                Remark = status switch
                {
                    STATUS_PAID_UN_TAKING => "订单支付",
                    STATUS_TAKING_UN_DO => "订单已接单",
                    STATUS_TAKEN => "订单执行完成",
                    STATUS_FINISH => "订单完成",
                    STATUS_CANCEL => "订单取消",
                }
            });
            db.SaveChanges();
        }

        internal static void IncludeUser(LegworkContext db, IUserRepository userStore, OrderListItem[] items)
        {
            var idItems = new HashSet<int>();
            foreach (var item in items)
            {
                if (item.UserId > 0)
                {
                    idItems.Add(item.UserId);
                }
                if (item.WaiterId > 0)
                {
                    idItems.Add(item.WaiterId);
                }
                if (item.ProviderId > 0)
                {
                    idItems.Add(item.ProviderId);
                }
            }
            if (idItems.Count == 0)
            {
                return;
            }
            var data = userStore.GetDictionary([.. idItems]);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.UserId > 0 && data.TryGetValue(item.UserId, out var user))
                {
                    item.User = user;
                }
                if (item.WaiterId > 0 && data.TryGetValue(item.WaiterId, out user))
                {
                    item.Waiter = user;
                }
                if (item.ProviderId > 0 && data.TryGetValue(item.ProviderId, out user))
                {
                    item.Provider = user;
                }
            }
        }
    }
}