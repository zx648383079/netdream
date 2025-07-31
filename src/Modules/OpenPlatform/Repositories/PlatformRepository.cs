using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Forms;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.OpenPlatform.Repositories
{
    public class PlatformRepository(
        OpenContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public const byte STATUS_NONE = 0;
        public const byte STATUS_WAITING = 9;
        public const byte STATUS_SUCCESS = 1;

        public IPage<PlatformListItem> GetList(PlatformQueryForm form)
        {
            var items = db.Platforms.Search(form.Keywords, "name")
                .When(form.User > 0, i => i.UserId == form.User)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            return items;
        }

        public IOperationResult<PlatformEntity> Get(int id)
        {
            var model = db.Platforms.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<PlatformEntity>.Fail("数据错误");
            }
            return OperationResult.Ok(model);
        }
        public IOperationResult<PlatformEntity> Save(PlatformForm data)
        {
            var model = data.Id > 0 ? db.Platforms.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new PlatformEntity();
            if (model is null)
            {
                return OperationResult.Fail<PlatformEntity>("id error");
            }
            if (model.UserId == 0)
            {
                model.UserId = client.UserId;
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Type = data.Type;
            model.Domain = data.Domain;
            model.SignKey = data.SignKey;
            model.SignType = data.SignType;
            model.EncryptType = data.EncryptType;
            model.PublicKey = data.PublicKey;
            model.AllowSelf = data.AllowSelf;
            db.Platforms.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Platforms.Where(i => i.Id == id).ExecuteDelete();
            db.PlatformOptions.Where(i => i.PlatformId == id).ExecuteDelete();
            db.UserTokens.Where(i => i.PlatformId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public void GenerateNewId(PlatformEntity entity)
        {
            entity.Appid = "1" + client.Now;
            entity.Secret = StrHelper.MD5Encode(Guid.NewGuid().ToString());
        }

        public IPage<PlatformListItem> SelfList(QueryForm form)
        {
            return db.Platforms.Search(form.Keywords, "name")
                .Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
        }

        /// <summary>
        /// 前台保存应用
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IOperationResult<PlatformEntity> SelfSave(PlatformForm data)
        {
            PlatformEntity? model;
            if (data.Id > 0)
            {
                model = db.Platforms.Where(i => i.Id == data.Id && i.UserId == client.UserId).Single();
                if (model is null)
                {
                    return OperationResult<PlatformEntity>.Fail("应用不存在");
                }
            }
            else
            {
                model = new PlatformEntity()
                {
                    UserId = client.UserId,
                    Status = STATUS_WAITING,
                };
                GenerateNewId(model);
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Domain = data.Domain;
            model.SignType = data.SignType;
            model.SignKey = data.SignKey;
            model.EncryptType = data.EncryptType;
            model.PublicKey = data.PublicKey;
            model.AllowSelf = data.AllowSelf;
            db.Platforms.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<PlatformEntity> SelfGet(int id)
        {
            var model = db.Platforms.Where(i => i.Id == id && i.UserId == client.UserId)
                .SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<PlatformEntity>("数据错误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult SelfRemove(int id)
        {
            var model = db.Platforms.Where(i => i.Id == id && i.UserId == client.UserId)
                .SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("数据错误");
            }
            db.Platforms.Remove(model);
            db.PlatformOptions.Where(i => i.PlatformId == id).ExecuteDelete();
            db.UserTokens.Where(i => i.PlatformId == id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public ListLabelItem[] SelfAll()
        {
            return db.Platforms.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => new ListLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                }).ToArray();
        }

        /// <summary>
        /// 创建 token
        /// </summary>
        /// <param name="platform_id"></param>
        /// <param name="expired_at"></param>
        /// <returns></returns>
        public IOperationResult<UserTokenEntity> CreateToken(TokenForm data)
        {
            if (data.Platform < 0)
            {
                return OperationResult<UserTokenEntity>.Fail("请选择应用");
            }
            var platform = db.Platforms.Where(i => i.Id == data.Platform 
            && i.AllowSelf == 1 && i.Status == 1).SingleOrDefault();
            if (platform is null)
            {
                return OperationResult<UserTokenEntity>.Fail("请选择应用");
            }
            var model = new UserTokenEntity()
            {
                UserId = client.UserId,
                PlatformId = data.Platform,
                Token = StrHelper.MD5Encode($"{client.UserId}:{TimeHelper.Millisecond()}"),
                ExpiredAt = string.IsNullOrWhiteSpace(data.ExpiredAt) ? client.Now + 86400 :
                    TimeHelper.TimestampFrom(data.ExpiredAt),
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
            };
            db.UserTokens.Add(model);
            db.SaveChanges();
            if (model.Id == 0)
            {
                return OperationResult<UserTokenEntity>.Fail("生成失败");
            }
            return OperationResult.Ok(model);
        }

        public IPage<TokenListItem> SelfTokenList(QueryForm form)
        {
            var items = db.UserTokens.When(form.Keywords, i => i.Token == form.Keywords)
                .Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            Include(items.Items);
            return items;
        }

        private void Include(IWithPlatformModel[] items)
        {
            var idItems = items.Select(item => item.PlatformId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Platforms.Where(i => idItems.Contains(i.Id)).ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.PlatformId > 0 && data.TryGetValue(item.PlatformId, out var res))
                {
                    item.Platform = res;
                }
            }
        }

        public void SelfTokenRemove(int id)
        {
            db.UserTokens.Where(i => i.UserId == client.UserId && i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public void SelfTokenClear()
        {
            db.UserTokens.Where(i => i.UserId == client.UserId).ExecuteDelete();
            db.SaveChanges();
        }

        public ListLabelItem[] SelfPlatformAll()
        {
            return db.Platforms.Where(i => i.AllowSelf == 1 && i.Status == STATUS_SUCCESS)
                .Select(i => new ListLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                }).ToArray();
        }
    }
}
