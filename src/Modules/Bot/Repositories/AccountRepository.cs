using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Bot.Entities;
using NetDream.Modules.Bot.Forms;
using NetDream.Modules.Bot.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Bot.Repositories
{
    public class AccountRepository(BotContext db, IClientContext client, IUserRepository userStore)
    {
        public IPage<BotListItem> GetList(QueryForm form)
        {
            return db.Bots.Search(form.Keywords, "name")
                .Where(i => i.Status == BotRepository.STATUS_ACTIVE)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form, i => i.SelectAs());
        }

        public IPage<BotListItem> ManageList(QueryForm form)
        {
            var items = db.Bots.Search(form.Keywords, "name", "account")
                .OrderByDescending(i => i.Status)
                .ThenByDescending(i => i.CreatedAt)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            return items;
        }

        public IPage<BotListItem> SelfList(QueryForm form)
        {
            return db.Bots.Search(form.Keywords, "name", "account")
                .Where(i => i.UserId == client.UserId)
               .OrderByDescending(i => i.CreatedAt)
               .ToPage(form, i => i.SelectAs());
        }

        /// <summary>
        /// TODO 需要限制一些信息显示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult<BotEntity> Get(int id)
        {
            var model = db.Bots.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<BotEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<BotEntity> SelfGet(int id)
        {
            var model = db.Bots.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<BotEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Bots.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public void SelfRemove(int id)
        {
            db.Bots.Where(i => i.Id == id && i.UserId == client.UserId).ExecuteDelete();
            db.SaveChanges();
        }

        public IOperationResult<BotEntity> Save(BotForm data)
        {
            var model = data.Id > 0 ? db.Bots.Where(i => i.Id == data.Id && i.UserId == client.UserId)
                .SingleOrDefault() :
                new BotEntity();
            if (model is null)
            {
                return OperationResult.Fail<BotEntity>("id error");
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Qrcode = data.Qrcode;
            model.Appid = data.Appid;
            model.Address = data.Address;
            model.Account = data.Account;
            model.AccessToken = data.AccessToken;
            model.AesKey = data.AesKey;
            model.Secret = data.Secret;
            model.Original = data.Original;
            model.Password = data.Password;
            model.PlatformType = data.PlatformType;
            model.Token = data.Token;
            db.Bots.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public bool IsSelf(int id)
        {
            return db.Bots.Where(i => i.Id == id && i.UserId == client.UserId).Any();
        }

        public static bool IsSelf(BotContext db, IClientContext client, int id)
        {
            return db.Bots.Where(i => i.Id == id && i.UserId == client.UserId).Any();
        }
    }
}
