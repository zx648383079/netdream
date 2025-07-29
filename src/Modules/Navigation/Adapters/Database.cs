using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Navigation.Forms;
using NetDream.Modules.Navigation.Models;
using NetDream.Modules.Navigation.Repositories;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Navigation.Adapters
{
    public class DatabaseAdapter(NavigationContext db,
        IUserRepository userStore,
        IClientContext client) : ISearchAdapter
    {
        public IOperationResult<PageModel> Create(PageForm data)
        {
            data.Id = 0;
            var store = new PageRepository(db, userStore, client);
            var res = store.Save(data);
            if (!res.Succeeded)
            {
                return OperationResult<PageModel>.Fail(res);
            }
            return OperationResult.Ok(store.Convert(res.Result));
        }

        public IOperationResult<PageModel> Get(int id)
        {
            return new PageRepository(db, userStore, client).Get(id);
        }

        public IOperationResult Remove(int id)
        {
            db.Pages.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IPage<PageListItem> Search(QueryForm form)
        {
            return new PageRepository(db, userStore, client).GetList(form);
        }

        public IOperationResult<PageModel> Update(int id, PageForm data)
        {
            data.Id = id;
            var store = new PageRepository(db, userStore, client);
            var res = store.Save(data);
            if (!res.Succeeded)
            {
                return OperationResult<PageModel>.Fail(res);
            }
            return OperationResult.Ok(store.Convert(res.Result));
        }

        public string[] Suggest(string keywords)
        {
            return db.Keywords.Search(keywords, "word").Take(10).Pluck(i => i.Word);
        }
    }
}
