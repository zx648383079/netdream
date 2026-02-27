using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserIdentity.Entities;
using NetDream.Modules.UserIdentity.Forms;
using NetDream.Modules.UserIdentity.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.UserIdentity.Repositories
{
    public class ZoneRepository(IdentityContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<ZoneEntity> GetList(QueryForm form)
        {
            var items = db.Zones.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
            return items;
        }

        public IOperationResult<ZoneEntity> Save(ZoneForm data)
        {
            var model = data.Id > 0 ? db.Zones.Where(i => i.Id == data.Id).SingleOrDefault() :
                new ZoneEntity();
            if (model is null)
            {
                return OperationResult.Fail<ZoneEntity>("id is error");
            }
            model.Name = data.Name;
            model.Icon = data.Icon;
            model.Description = data.Description;
            model.Status = data.Status;
            model.IsOpen = data.IsOpen;
            db.Zones.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Zones.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public ZoneChangeModel UserZone()
        {
            if (!int.TryParse(userStore.GetAttached(client.UserId, "zone_at"), out var lastAt))
            {
                lastAt = 0;
            }
            var data = All();
            var activated_at = 0;
            var selected = Array.Empty<ZoneListItem>();
            if (lastAt > 0)
            {
                var zoneId = int.Parse(userStore.GetAttached(client.UserId, "zone_id")!);

                selected = data.Where(i => i.Id == zoneId).ToArray();
                activated_at = lastAt + 30 * 86400;
                if (activated_at <= client.Now)
                {
                    activated_at = 0;
                }
            }
            return new ZoneChangeModel()
            {
                Data = data,
                ActivatedAt = activated_at,
                Selected = selected
            };
        }

        public ZoneListItem[] All()
        {
            return db.Zones.Where(i => i.Status == 1).OrderBy(i => i.Id).SelectAs().ToArray();
        }

        public IOperationResult Save(int user, int zoneId)
        {
            return UserChange(user, zoneId, true);
        }

        public IOperationResult UserChange(int user, int zoneId, bool checkTime = true)
        {
            var text = userStore.GetAttached(user, "zone_at");
            if (!int.TryParse(text, out var lastAt))
            {
                lastAt = 0;
            }
            if (checkTime && lastAt > 0 && lastAt > client.Now - 30 * 86400)
            {
                return OperationResult.Fail("时间间隔不允许设置");
            }
            if (text is null)
            {
                userStore.Attach(user, "zone_at", client.Now.ToString());
                userStore.Attach(user, "zone_id", zoneId.ToString());
                return OperationResult.Ok();
            }
            if (checkTime)
            {
                userStore.Attach(user, "zone_at", client.Now.ToString());
            }
            userStore.Attach(user, "zone_id", zoneId.ToString());
            return OperationResult.Ok();
        }
    }
}
