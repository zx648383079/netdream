using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Contact.Repositories
{
    public class FriendLinkRepository(ContactContext db, IClientContext environment)
    {
        public IPage<FriendLinkEntity> GetList(string keywords = "", int page = 1)
        {
            return db.FriendLinks.Search(keywords, "name", "email", "url", "brief")
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id).ToPage(page);
        }

        public FriendLinkEntity? Get(int id)
        {
            return db.FriendLinks.Where(i => i.Id == id).Single();
        }

        public IOperationResult<FriendLinkEntity> Save(FriendLinkForm data)
        {
            var model = data.Id > 0 ? db.FriendLinks.Where(i => i.Id == data.Id).Single() :
                new FriendLinkEntity();
            if (model is null)
            {
                return OperationResult.Fail<FriendLinkEntity>("id is error");
            }
            model.Name = data.Name;
            model.Email = data.Email;
            model.Url = data.Url;
            model.Brief = data.Brief;
            if (model.UserId == 0)
            {
                model.UserId = environment.UserId;
            }
            db.FriendLinks.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public FriendLinkEntity? Toggle(int id, string remark)
        {
            var model = Get(id);
            if (model is null)
            {
                return null;
            }
            model.Status = model.Status > 0 ? 0 : 1;
            db.FriendLinks.Save(model);
            db.SaveChanges();
            //event (new ManageAction("friend_link",
            //    sprintf("友情链接[%d]:%s，理由: %s", model.Url,
            //        model.Status > 0 ? "上架" : "下架", remark)
            //    , Constants.TYPE_SYSTEM_FRIEND_LINK, id));
            // SEORepository.ClearCache(["pages", "nodes"]);
            return model;
        }

        public void Remove(params int[] id)
        {
            db.FriendLinks.Where(i => id.Contains(i.Id)).ExecuteDelete();
        }
}
}
