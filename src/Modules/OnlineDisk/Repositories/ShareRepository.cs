using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineDisk.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.OnlineDisk.Repositories
{
    public class ShareRepository(OnlineDiskContext db, 
        IClientContext client, 
        IUserRepository userStore)
    {
        public const byte SHARE_PUBLIC = 0; //公开分享
        public const byte SHARE_PROTECTED = 1; //密码分享
        public const byte SHARE_PRIVATE = 2;  //分享给个人
        public IPage<ShareListItem> PublicList(QueryForm form)
        {
            var items = db.Shares.Search(form.Keywords, "name")
                .Where(i => i.Mode == SHARE_PUBLIC)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            return items;
        }

        public IPage<ShareListItem> MyList(QueryForm form)
        {
            var items = db.Shares.Search(form.Keywords, "name")
                .Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            return items;
        }

        public IOperationResult Remove(params int[] idItems)
        {
            if (idItems.Length == 0)
            {
                return OperationResult.Fail("数据错误");
            }
            idItems = db.Shares.Where(i => idItems.Contains(i.Id) && i.UserId == client.UserId)
                .Pluck(i => i.Id);
            if (idItems.Length == 0)
            {
                return OperationResult.Fail("数据错误");
            }
            db.Shares.Where(i => idItems.Contains(i.Id)).ExecuteDelete();
            db.ShareFiles.Where(i => idItems.Contains(i.ShareId)).ExecuteDelete();
            db.ShareUsers.Where(i => idItems.Contains(i.ShareId)).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }
    }
}
