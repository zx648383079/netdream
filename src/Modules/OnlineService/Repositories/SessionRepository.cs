using Modules.OnlineService.Entities;
using NetDream.Modules.OnlineService.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NPoco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OnlineService.Repositories
{
    public class SessionRepository(IDatabase db, 
        IClientContext environment,
        IUserRepository userStore)
    {
        public Page<SessionModel> GetList(string keywords = "", int status = 0, int page = 1)
        {
            var sql = new Sql().Select("*").From<SessionEntity>(db);
            SearchHelper.Where(sql, ["name", "ip"], keywords);
            if (status > 0)
            {
                sql.Where("status=@0", status - 1);
            }
            sql.OrderBy("id DESC");
            var items = db.Page<SessionModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            return items;
        }

        public IList<SessionModel> MyList()
        {
            var data = db.Fetch<SessionModel>(
                "WHERE (status=0 AND updated_at>@0) OR (status>0 AND service_id=@1 AND updated_at>@2) ORDER BY id DESC",
                environment.Now - 3600, environment.UserId, environment.Now - 86400);
            userStore.WithUser(data);
            var guest = new GuestUser();
            foreach (var item in data)
            {
                item.User ??= guest;
            }
            return data;
        }

        public SessionEntity Remark(int sessionId, string remark)
        {
            var model = db.SingleById<SessionEntity>(sessionId);
            if (model is null)
            {
                throw new Exception("错误");
            }
            model.Remark = remark;
            db.TrySave(model);
            db.Insert(new SessionLogEntity()
            {
                UserId = environment.UserId,
                SessionId = sessionId,
                Status = model.Status,
                Remark = string.Format("客服 【{0}】 修改了备注：{1}", environment.UserId, remark)
            });
            return model;
        }

        public SessionEntity Transfer(int sessionId, int user)
        {
            var model = db.SingleById<SessionEntity>(sessionId);
            if (model is null)
            {
                throw new Exception("错误");
            }
            if (!new CategoryRepository(db, userStore).HasService(user))
            {
                throw new Exception("客服错误");
            }
            model.ServiceId = user;
            model.ServiceWord = 0;
            db.TrySave(model);
            db.Insert(new SessionLogEntity()
            {
                UserId = environment.UserId,
                SessionId = sessionId,
                Status = model.Status,
                Remark = string.Format("客服 【{0}】 转交了会话给客服【{1}】", environment.UserId, userStore.Get(user)?.Name)
            });
            return model;
        }

        /**
         * 设置自动回复
         * @param int sessionId
         * @param int user
         * @throws \Exception
         */
        public SessionEntity Reply(int sessionId, int word)
        {
            var model = db.SingleById<SessionEntity>(sessionId);
            if (model is null)
            {
                throw new Exception("错误");
            }
            if (word > 0 && !new CategoryRepository(db, userStore).HasWord(word))
            {
                throw new Exception("自动回复语错误");
            }
            model.ServiceWord = word;
            db.TrySave(model);
            return model;
        }

        /**
         * 判断客服是否有权限查看
         * @param int sessionId
         * @return bool
         * @throws \Exception
         */
        public bool HasRole(int sessionId)
        {
            var model = db.SingleById<SessionEntity>(sessionId);
            if (model is null)
            {
                return false;
            }
            if (model.ServiceId < 1)
            {
                return true;
            }
            return model.ServiceId == environment.UserId;
        }
    }
}
