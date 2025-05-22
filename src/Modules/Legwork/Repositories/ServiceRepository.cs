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
using System.Text.Json;

namespace NetDream.Modules.Legwork.Repositories
{
    public class ServiceRepository(LegworkContext db, 
        IClientContext client,
        IUserRepository userStore)
    {


        public IPage<ServiceListItem> GetList(QueryForm form, 
            int cat_id = 0, int user_id = 0, int status = 0, bool full = true)
        {
            var res = db.Service.Search(form.Keywords, "name")
                .When(user_id > 0, i => i.UserId == user_id)
                .When(cat_id > 0, i => i.CatId == cat_id)
                .When(status > 0, i => i.Status == LegworkRepository.STATUS_ALLOW)
                .ToPage(form).CopyTo<ServiceEntity, ServiceListItem>();
            userStore.Include(res.Items);
            CategoryRepository.Include(db, res.Items);
            return res;
        }

        public IPage<ServiceListItem> GetSelfList(QueryForm form)
        {
            var res = db.Service.Search(form.Keywords, "name")
                .Where(i => i.UserId == client.UserId)
                .ToPage(form).CopyTo<ServiceEntity, ServiceListItem>();
            CategoryRepository.Include(db, res.Items);
            return res;
        }

        public IOperationResult<ServiceModel> Get(int id)
        {
            var model = db.Service.Where(i => i.Id == id).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<ServiceModel>.Fail("数据有误");
            }
            return OperationResult.Ok(model.CopyTo<ServiceModel>());
        }

        public IOperationResult<ServiceModel> GetSelf(int id)
        {
            var model = db.Service.Where(i => i.Id == id && i.UserId == client.UserId).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<ServiceModel>.Fail("数据有误");
            }
            return OperationResult.Ok(model.CopyTo<ServiceModel>());
        }

        public IOperationResult<ServiceModel> GetPublic(int id)
        {
            var model = db.Service.Where(i => i.Id == id && i.Status == LegworkRepository.STATUS_ALLOW).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<ServiceModel>.Fail("无权限操作");
            }
            var res = model.CopyTo<ServiceModel>();
            res.Category = db.Categories.Where(i => i.Id == model.CatId)
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .FirstOrDefault();
            var provider = db.Provider.Where(i => i.UserId == model.UserId)
                .FirstOrDefault();
            if (provider is not null)
            {
                res.Provider = new()
                {
                    Id = provider.Id,
                    Name = provider.Name,
                    Tel = StrHelper.HideTel(provider.Tel),
                    Address = provider.Address,
                    Logo = provider.Logo,
                    OverallRating = provider.OverallRating,
                    CreatedAt = provider.CreatedAt,
                };
            }
            return OperationResult.Ok(res);
        }

        public IOperationResult<ServiceEntity> Change(int id, byte status)
        {
            var model = db.Service.Where(i => i.Id == id).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<ServiceEntity>.Fail("数据有误");
            }
            model.Status = status;
            db.Service.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<ServiceEntity> Save(ServiceForm data)
        {
            var model = data.Id > 0 ? db.Service.Where(i => i.Id == data.Id && i.UserId == client.UserId).FirstOrDefault() : new ServiceModel();
            if (model is null)
            {
                return OperationResult<ServiceEntity>.Fail("数据有误");
            }
            model.Name = data.Name;
            model.Thumb = data.Thumb;
            model.Brief = data.Brief;
            model.Price = data.Price;
            model.Content = data.Content;
            model.Form = JsonSerializer.Serialize(data.Form ?? []);
            model.UserId = client.UserId;
            model.Status = 0;
            db.Service.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Service.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public void RemoveSelf(int id)
        {
            db.Service.Where(i => i.Id == id && i.UserId == client.UserId).ExecuteDelete();
            db.SaveChanges();
        }

        public bool IsAllowWaiter(int id, int user_id)
        {
            return db.ServiceWaiter.Where(i => i.UserId == user_id && i.ServiceId == id && i.Status == LegworkRepository.STATUS_ALLOW)
                    .Any();
        }

        public IPage<UserListItem> WaiterList(QueryForm form, int id, int status = 0)
        {
            var links = db.ServiceWaiter
                .Where(i => i.ServiceId == id)
                .When(status > 0, i => i.Status == 1)
                .Select(i => new KeyValuePair<int, byte>(i.UserId, i.Status)).ToDictionary();
            if (links.Count == 0)
            {
                return new Page<UserListItem>(0, form);
            }
            var data = userStore.Search(form, links.Keys.ToArray(), false);

            return new Page<UserListItem>(data)
            {
                Items = data.Items.Select(i => new UserListItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Avatar = i.Avatar,
                    IsOnline = i.IsOnline,
                    Status = links[i.Id]
                }).ToArray()
            };
        }

        public IOperationResult WaiterChange(int id, int[] users, byte status = 0)
        {
            if (!new ProviderRepository(db, client, userStore).HasService(id))
            {
                return OperationResult.Fail("服务错误");
            }
            db.ServiceWaiter.Where(i => i.ServiceId == id && users.Contains(i.UserId))
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 0));
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult WaiterAdd(int id, int[] users, byte status = 0)
        {
            if (!new ProviderRepository(db, client, userStore).HasService(id))
            {
                return OperationResult.Fail("服务错误");
            }
            var exist = db.ServiceWaiter.Where(i => i.ServiceId == id)
                .Select(i => i.UserId).ToArray();
            var add = ModelHelper.Diff(users, exist);
            var update = ModelHelper.Diff(exist, users);
            if (add.Length > 0)
            {
                foreach (var item in add)
                {
                    db.ServiceWaiter.Add(new()
                    {
                        UserId = item,
                        Status = status,
                        ServiceId = id
                    });
                }
            }
            if (update.Length > 0)
            {
                db.ServiceWaiter.Where(i =>
                    i.ServiceId == id && update.Contains(i.UserId)
                    && i.Status != LegworkRepository.STATUS_DISALLOW)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 0));
            }
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult WaiterRemove(int id, int[] users)
        {
            if (!new ProviderRepository(db, client, userStore).HasService(id))
            {
                return OperationResult.Fail("服务错误");
            }
            db.ServiceWaiter.Where(i => i.ServiceId == id && users.Contains(i.UserId))
                .ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        internal static void Include(LegworkContext db, IWithServiceModel[] items)
        {
            var idItems = items.Select(i => i.ServiceId).Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Service.Where(i => idItems.Contains(i.Id))
                .Select(i => new ServiceLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    Thumb = i.Thumb,
                }).ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach ( var item in items)
            {
                if (data.TryGetValue(item.ServiceId, out var it))
                {
                    item.Service = it;
                }
            }
        }

        internal static bool IsAllowWaiter(LegworkContext db, int serviceId, int waiterId)
        {
            return db.ServiceWaiter.Where(i => i.UserId == waiterId && i.ServiceId == serviceId && i.Status == LegworkRepository.STATUS_ALLOW)
                    .Any();
        }
    }
}