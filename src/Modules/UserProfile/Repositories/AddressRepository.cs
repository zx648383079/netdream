using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserProfile.Entities;
using NetDream.Modules.UserProfile.Forms;
using NetDream.Modules.UserProfile.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.UserProfile.Repositories
{
    public class AddressRepository(ProfileContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        const string DEFAULT_KEY = "address_id";
        public int DefaultId {
            get => int.Parse(userStore.GetAttached(client.UserId, DEFAULT_KEY));
            set => userStore.Attach(client.UserId, DEFAULT_KEY, value.ToString());
        }

        public IPage<AddressListItem> GetList(QueryForm form)
        {
            var res = db.Address.Where(i => i.UserId == client.UserId)
                .Search(form.Keywords, "name", "address", "tel")
                .OrderByDescending(i => i.Id).ToPage<AddressEntity, AddressListItem>(form);
            RegionRepository.Include(db, res.Items);
            var defaultId = DefaultId;
            foreach (var item in res.Items)
            {
                item.IsDefault = item.Id == defaultId;
            }
            return res;
        }

        public IOperationResult<AddressListItem> Get(int id)
        {
            var model = db.Address.Where(i => i.UserId == client.UserId && i.Id == id)
                .SingleOrDefault();
            if (model == null)
            {
                return OperationResult<AddressListItem>.Fail("数据错误");
            }
            var res = model.CopyTo<AddressListItem>();
            res.Region = db.Regions.Where(i => i.Id == model.RegionId)
                .SingleOrDefault();
            res.IsDefault = model.Id == DefaultId;
            return OperationResult.Ok(res);
        }

        public void Remove(int id)
        {
            db.Address.Where(i => i.UserId == client.UserId && i.Id == id).ExecuteDelete();
        }

        /**
         * 保存地址
         * @param array data
         * @return Address
         * @throws Exception
         */
        public IOperationResult<AddressEntity> Save(AddressForm data)
        {
            if (data.RegionId < 1 && !string.IsNullOrWhiteSpace(data.RegionName))
            {
                data.RegionId = db.Regions.Where(i => i.Name == data.RegionName).Value(i => i.Id);
            }
            var model = data.Id > 0 ? db.Address.Where(i => i.UserId == client.UserId && i.Id == data.Id)
                .SingleOrDefault() : new AddressEntity();
            if (model == null)
            {
                return OperationResult<AddressEntity>.Fail("数据错误");
            }
            if (data.Id == 0 || !data.Tel.Contains("**"))
            {
                model.Tel = data.Tel;
            }
            model.Name = data.Name;
            model.Address = data.Address;
            model.UserId = client.UserId;
            model.RegionId = data.RegionId;
            db.Address.Save(model, client.Now);
            db.SaveChanges();
            if (data.IsDefault)
            {
                DefaultId = model.Id;
            }
            return OperationResult.Ok(model);
        }

        /**
         * 设为默认
         * @param id
         * @return int
         * @throws Exception
         */
        public IOperationResult SetDefault(int id)
        {
            if (!db.Address.Where(i => i.UserId == client.UserId && i.Id == id).Any())
            {
                return OperationResult.Fail("地址错误");
            }
            DefaultId = id;
            return OperationResult.Ok();
        }

        public IOperationResult<AddressEntity> GetDefault()
        {
            var id = DefaultId;
            AddressEntity? model = null;
            if (id > 0)
            {
                model = db.Address.Where(i => i.Id == id && i.UserId == client.UserId)
                    .SingleOrDefault();
            }
            if (model is null)
            {
                model = db.Address.Where(i => i.UserId == client.UserId)
                    .FirstOrDefault();
            }
            if (model is null)
            {
                return OperationResult.Fail<AddressEntity>("请先添加地址");
            }
            return OperationResult.Ok(model);
        }

    }
}
