using NetDream.Modules.Legwork.Entities;
using NetDream.Modules.Legwork.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Legwork.Repositories
{
    public class OrderRepository(LegworkContext db, 
        IClientContext client,
        IUserRepository userStore)
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
                    .Where(i => i.Status == ServiceRepository.STATUS_ALLOW)
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
            IncludeUser(res.Items);
            return res;
        }

        private void IncludeUser(OrderListItem[] items)
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

        public IPage<OrderListItem> GetSelfList(QueryForm form, int status = 0, int id = 0)
        {
            var serviceId = Array.Empty<int>();
            if (string.IsNullOrWhiteSpace(form.Keywords))
            {
                serviceId = db.Service.Search(form.Keywords, "name")
                    .Where(i => i.Status == ServiceRepository.STATUS_ALLOW)
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
            IncludeUser(res.Items);
            return res;
        }

        
    }
}