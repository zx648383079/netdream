using NetDream.Modules.OnlineService.Entities;
using NetDream.Modules.OnlineService.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.OnlineService.Repositories
{
    public class SessionRepository(OnlineServiceContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<SessionModel> GetList(string keywords = "", int status = 0, int page = 1)
        {
            var items = db.Sessions.Search(keywords, "name", "ip")
                .When(status > 0, i => i.Status == status - 1)
                .OrderByDescending(i => i.Id).ToPage(page).CopyTo<SessionEntity, SessionModel>();
            userStore.WithUser(items.Items);
            return items;
        }

        public SessionModel[] MyList()
        {
            var data = db.Sessions.Where(i => (i.Status == 0 && i.UpdatedAt> client.Now - 3600) 
                || (i.Status > 0 && i.ServiceId == client.UserId && i.UpdatedAt > client.Now - 86400))
                .OrderByDescending(i => i.Id).ToArray().CopyTo<SessionEntity, SessionModel>();
            userStore.WithUser(data);
            var guest = new GuestUser();
            foreach (var item in data)
            {
                item.User ??= guest;
            }
            return data;
        }

        public IOperationResult<SessionEntity> Remark(int sessionId, string remark)
        {
            var model = db.Sessions.Where(i => i.Id == sessionId).Single();
            if (model is null)
            {
                return OperationResult.Fail<SessionEntity>("错误");
            }
            model.Remark = remark;
            db.Sessions.Save(model, client.Now);
            db.SessionLogs.Save(new SessionLogEntity()
            {
                UserId = client.UserId,
                SessionId = sessionId,
                Status = model.Status,
                Remark = string.Format("客服 【{0}】 修改了备注：{1}", client.UserId, remark)
            }, client.Now);
            return OperationResult.Ok(model);
        }

        public IOperationResult<SessionEntity> Transfer(int sessionId, int user)
        {
            var model = db.Sessions.Where(i => i.Id == sessionId).Single();
            if (model is null)
            {
                return OperationResult.Fail<SessionEntity>("错误");
            }
            if (!new CategoryRepository(db, userStore).HasService(user))
            {
                return OperationResult.Fail<SessionEntity>("客服错误");
            }
            model.ServiceId = user;
            model.ServiceWord = 0;
            db.Sessions.Save(model, client.Now);
            db.SessionLogs.Save(new SessionLogEntity()
            {
                UserId = client.UserId,
                SessionId = sessionId,
                Status = model.Status,
                Remark = string.Format("客服 【{0}】 转交了会话给客服【{1}】", client.UserId, userStore.Get(user)?.Name)
            }, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /**
         * 设置自动回复
         * @param int sessionId
         * @param int user
         * @throws \Exception
         */
        public IOperationResult<SessionEntity> Reply(int sessionId, int word)
        {
            var model = db.Sessions.Where(i => i.Id == sessionId).Single();
            if (model is null)
            {
                return OperationResult.Fail<SessionEntity>("错误");
            }
            if (word > 0 && !new CategoryRepository(db, userStore).HasWord(word))
            {
                return OperationResult.Fail<SessionEntity>("自动回复语错误");
            }
            model.ServiceWord = word;
            db.Sessions.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /**
         * 判断客服是否有权限查看
         * @param int sessionId
         * @return bool
         * @throws \Exception
         */
        public bool HasRole(int sessionId)
        {
            var model = db.Sessions.Where(i => i.Id == sessionId).Single();
            if (model is null)
            {
                return false;
            }
            if (model.ServiceId < 1)
            {
                return true;
            }
            return model.ServiceId == client.UserId;
        }
    }
}
