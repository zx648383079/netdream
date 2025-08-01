using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Contact.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Contact.Repositories
{
    public class SubscribeRepository(ContactContext db)
    {
        public IPage<SubscribeEntity> GetList(QueryForm form)
        {
            return db.Subscribes.Search(form.Keywords, "email", "name")
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult Change(int[] id, int status)
        {
            if (id.Length == 0)
            {
                return OperationResult.Fail("数据错误");
            }
            db.Subscribes.Where(i => id.Contains(i.Id))
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, status));
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public void Remove(params int[] id)
        {
            db.Subscribes.Where(i => id.Contains(i.Id)).ExecuteDelete();
            db.SaveChanges();
        }
    }
}
