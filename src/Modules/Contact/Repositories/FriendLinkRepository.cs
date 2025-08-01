using MediatR;
using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Notifications;
using NetDream.Shared.Providers;
using NetDream.Shared.Repositories;
using System.Linq;

namespace NetDream.Modules.Contact.Repositories
{
    public class FriendLinkRepository(ContactContext db, 
        IClientContext client, IMediator mediator)
    {
        public IPage<FriendLinkEntity> GetList(QueryForm form)
        {
            return db.FriendLinks.Search(form.Keywords, "name", "email", "url", "brief")
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id).ToPage(form);
        }

        public IOperationResult<FriendLinkEntity> Get(int id)
        {
            var model = db.FriendLinks.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<FriendLinkEntity>("id is error");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<FriendLinkEntity> Save(FriendLinkForm data)
        {
            var model = data.Id > 0 ? db.FriendLinks.Where(i => i.Id == data.Id).SingleOrDefault() :
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
                model.UserId = client.UserId;
            }
            db.FriendLinks.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<FriendLinkEntity> Toggle(int id, string remark = "")
        {
            var model = db.FriendLinks.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<FriendLinkEntity>.Fail("数据错误");
            }
            model.Status = model.Status > 0 ? 0 : 1;
            db.FriendLinks.Save(model);
            db.SaveChanges();
            
            mediator.Publish(ManageAction.Create(client, "friend_link",
                string.Format("友情链接[{0}]:{1}，理由: {2}", model.Url,
                    model.Status > 0 ? "上架" : "下架", remark)
                , ModuleModelType.TYPE_SYSTEM_FRIEND_LINK, id));
            // SEORepository.ClearCache(["pages", "nodes"]);
            return OperationResult.Ok(model);
        }

        public void Remove(params int[] id)
        {
            db.FriendLinks.Where(i => id.Contains(i.Id)).ExecuteDelete();
        }
}
}
