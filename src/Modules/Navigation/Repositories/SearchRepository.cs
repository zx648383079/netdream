using NetDream.Modules.Navigation.Adapters;
using NetDream.Modules.Navigation.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System.Collections.Generic;

namespace NetDream.Modules.Navigation.Repositories
{
    public class SearchRepository(NavigationContext db,
        IUserRepository userStore,
        IClientContext client): ISearcher
    {
        public ISearchAdapter Searcher => new DatabaseAdapter(db, userStore, client);

        public IEnumerable<string> Cut(string text)
        {
            throw new System.NotImplementedException();
        }

        public IPage<PageListItem> GetList(QueryForm form)
        {
            return Searcher.Search(form);
        }

        public void Index(ModuleTargetType type, int id, string[] words)
        {
            throw new System.NotImplementedException();
        }

        public void Search(PaginationForm form)
        {
            throw new System.NotImplementedException();
        }

        public string[] Suggest(string keywords = "")
        {
            return Searcher.Suggest(keywords);
        }
    }
}
