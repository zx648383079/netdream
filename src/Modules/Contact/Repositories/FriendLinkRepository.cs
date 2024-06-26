using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Forms;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NPoco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Contact.Repositories
{
    public class FriendLinkRepository(IDatabase db, IClientEnvironment environment)
    {
        public Page<FriendLinkEntity> GetList(string keywords = "", int page = 1)
        {
            var sql = new Sql();
            sql.Select("*").From<FriendLinkEntity>(db);
            SearchHelper.Where(sql, ["name", "email", "url", "brief"], keywords);
            sql.OrderBy("status ASC", "id DESC");
            return db.Page<FriendLinkEntity>(page, 20, sql);
        }

        public FriendLinkEntity Get(int id)
        {
            return db.SingleById<FriendLinkEntity>(id);
        }

        public FriendLinkEntity Save(FriendLinkForm data)
        {
            var model = data.Id > 0 ? db.SingleById<FriendLinkEntity>(data.Id) :
                new FriendLinkEntity();
            model.Name = data.Name;
            model.Email = data.Email;
            model.Url = data.Url;
            model.Brief = data.Brief;
            if (model.UserId == 0)
            {
                model.UserId = environment.UserId;
            }
            db.TrySave(model);
            return model;
        }

        public FriendLinkEntity Toggle(int id, string remark)
        {
            var model = Get(id);
            model.Status = model.Status > 0 ? 0 : 1;
            db.TrySave(model);
            //event (new ManageAction("friend_link",
            //    sprintf("友情链接[%d]:%s，理由: %s", model.Url,
            //        model.Status > 0 ? "上架" : "下架", remark)
            //    , Constants.TYPE_SYSTEM_FRIEND_LINK, id));
            // SEORepository.ClearCache(["pages", "nodes"]);
            return model;
        }

        public void Remove(params int[] id)
        {
            db.DeleteById<FriendLinkEntity>(id);
        }
}
}
