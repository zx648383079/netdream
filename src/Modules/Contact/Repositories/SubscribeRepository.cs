using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Contact.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Contact.Repositories
{
    public class SubscribeRepository(ContactContext db)
    {
        public IPage<SubscribeEntity> GetList(string keywords = "", int page = 1)
        {
            return db.Subscribes.Search(keywords, "email", "name")
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id).ToPage(page);
        }

        public void Change(int[] id, int status)
        {
            if (id.Length == 0)
            {
                return;
            }
            db.Subscribes.Where(i => id.Contains(i.Id))
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, status));
        }

        public void Remove(params int[] id)
        {
            db.Subscribes.Where(i => id.Contains(i.Id)).ExecuteDelete();
        }
    }
}
