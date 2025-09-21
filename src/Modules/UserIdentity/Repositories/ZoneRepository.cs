using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserIdentity.Entities;
using NetDream.Modules.UserIdentity.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.UserIdentity.Repositories
{
    public class ZoneRepository(IdentityContext db, IClientContext client)
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
    }
}
