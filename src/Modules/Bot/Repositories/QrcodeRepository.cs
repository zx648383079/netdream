using NetDream.Modules.Bot.Entities;
using NetDream.Modules.Bot.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Bot.Repositories
{
    public class QrcodeRepository(BotContext db, IClientContext client)
    {
        public IPage<QrcodeEntity> GetList(int bot_id, QueryForm form)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return new Page<QrcodeEntity>();
            }
            return db.Qrcode.Where(i => i.BotId == bot_id)
                .Search(form.Keywords, "name").ToPage(form);
        }

        public IPage<QrcodeEntity> ManageList(int bot_id, QueryForm form)
        {
            return db.Qrcode.When(bot_id > 0, i => i.BotId == bot_id)
                .Search(form.Keywords, "name").ToPage(form);
        }

        public IOperationResult<QrcodeEntity> Get(int id)
        {
            var model = db.Qrcode.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<QrcodeEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<QrcodeEntity> GetSelf(int id)
        {
            var model = db.Qrcode.Where(i => i.Id == id).SingleOrDefault();
            if (model is null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail<QrcodeEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Qrcode.Where(i => i.Id == id).SingleOrDefault();
            if (model is null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail<QrcodeEntity>("数据有误");
            }
            db.Qrcode.Remove(model);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult<QrcodeEntity> Save(int bot_id, QrcodeForm data)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail<QrcodeEntity>("模板不存在");
            }
            var model = data.Id > 0 ?
                db.Qrcode.Where(i => i.Id == data.Id && i.BotId == bot_id)
                .SingleOrDefault() : new QrcodeEntity();
            if (model is null)
            {
                return OperationResult.Fail<QrcodeEntity>("模板不存在");
            }
            model.BotId = bot_id;
            model.Name = data.Name;
            model.SceneStr = data.SceneStr;
            model.QrUrl = data.QrUrl;
            model.Url = data.Url;
            model.ExpireTime = data.ExpireTime;
            model.SceneId = data.SceneId;
            model.SceneType = data.SceneType;
            db.Qrcode.Save(model, client.Now);
            db.SaveChanges();
            if (string.IsNullOrWhiteSpace(model.Url))
            {
                Sync(model);
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult Sync(QrcodeEntity model)
        {
            var res = new BotRepository().Entry(model.BotId)
                .PushQr(model);
            // TODO

            return res;
        }
    }
}
