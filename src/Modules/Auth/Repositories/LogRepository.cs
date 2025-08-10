using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.UserAccount.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Auth.Repositories
{
    public class LogRepository(AuthContext db, IClientContext client)
    {
        public IPage<LoginLogEntity> LoginLog(LogQueryForm form)
        {
            return db.LoginLogs.Search(form.Keywords, "ip")
                .When(form.User > 0, i => i.UserId == form.User)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form);
        }

        public AuthorizeListItem[] SelfAuthorize()
        {
            return [];
        }

        public ConnectListItem[] SelfConnect()
        {
            var items = db.OAuth.Where(i => i.UserId == client.UserId)
                .Select(i => new OauthEntity()
                {
                    Id = i.Id,
                    Vendor = i.Vendor,
                    PlatformId = i.PlatformId,
                    Nickname = i.Nickname,
                    CreatedAt = i.CreatedAt
                }).ToArray();

            return [];
        }

        public IOperationResult SelfConnectRemove(int id)
        {
            if (id < 1)
            {
                return OperationResult.Fail("请选择解绑的平台");
            }
            db.OAuth.Where(i => i.Id == id && i.UserId == client.UserId).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public DriverListItem[] SelfDriver()
        {
            return [];
        }

        public IPage<LoginLogEntity> SelfLoginLog(LogQueryForm form)
        {
            return db.LoginLogs.Search(form.Keywords, "ip")
                .Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form);
        }
    }
}
