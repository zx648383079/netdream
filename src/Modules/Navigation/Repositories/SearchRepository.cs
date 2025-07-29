using NetDream.Modules.Navigation.Adapters;
using NetDream.Modules.Navigation.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Modules.Navigation.Repositories
{
    public class SearchRepository(NavigationContext db,
        IUserRepository userStore,
        IClientContext client)
    {
        public ISearchAdapter Searcher => new DatabaseAdapter(db, userStore, client);

        public IPage<PageListItem> GetList(QueryForm form)
        {
            return Searcher.Search(form);
        }

        public string[] Suggest(string keywords = "")
        {
            return Searcher.Suggest(keywords);
        }
    }
}
