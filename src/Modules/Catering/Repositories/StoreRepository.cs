using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Catering.Entities;
using NetDream.Modules.Catering.Forms;
using NetDream.Modules.Catering.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Catering.Repositories
{
    public class StoreRepository(CateringContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<StoreListItem> ManageGetList(QueryForm form, int user = 0)
        {
            var res = db.Store.Search(form.Keywords, "name")
                .When(user > 0, i => i.UserId == user)
                .OrderByDescending(i => i.Id)
                .ToPage(form).CopyTo<StoreEntity, StoreListItem>();
            userStore.WithUser(res.Items);
            return res;
        }

        public IOperationResult<StoreListItem> ManageGet(int id)
        {
            var model = db.Store.Where(i => i.Id == id).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<StoreListItem>.Fail("note error");
            }
            var res = model.CopyTo<StoreListItem>();
            userStore.WithUser(res);
            return OperationResult.Ok(res);
        }

        public IOperationResult<StoreEntity> ManageSave(StoreForm data)
        {
            var model = data.Id > 0 ? db.Store.Where(i => i.Id == data.Id)
                .FirstOrDefault() : new StoreEntity();
            if (model is null)
            {
                return OperationResult<StoreEntity>.Fail("error");
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Logo = data.Logo;
            model.Address = data.Address;
            model.OpenStatus = data.OpenStatus;
            model.UserId = client.UserId;
            db.Store.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void ManageRemove(int id)
        {
            db.Store.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public StoreStatus Profile()
        {
            if (client.UserId == 0)
            {
                return new();
            }
            var hasStore = db.Store.Where(i => i.UserId == client.UserId).Any();
            var isWaiter = hasStore || db.StoreStaff.Where(i => i.UserId == client.UserId).Any();
            return new()
            {
                HasStore = hasStore,
                IsWaiter = isWaiter
            };
        }

        public IOperationResult<StoreModel> MerchantGet()
        {
            var model = db.Store.Where(i => i.UserId == client.UserId).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<StoreModel>.Fail("store is error");
            }
            var data = model.CopyTo<StoreModel>();
            data.MetaItems = new MetaRepository(db).GetOrDefault(model.Id);
            return OperationResult.Ok(data);
        }

        public IOperationResult<StoreEntity> MerchantSave(StoreForm data)
        {
            var model = db.Store.Where(i => i.UserId == client.UserId).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<StoreEntity>.Fail("store is error");
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Logo = data.Logo;
            model.Address = data.Address;
            model.OpenStatus = data.OpenStatus;
            model.UserId = client.UserId;
            db.Store.Save(model, client.Now);
            db.SaveChanges();
            // new MetaRepository(db).SaveBatch(model.Id, data);
            return OperationResult.Ok(model);
        }


        public static int Own(CateringContext db, IClientContext client)
        {
            return db.Store.Where(i => i.UserId == client.UserId).Select(i => i.Id).FirstOrDefault();
        }
    }
}