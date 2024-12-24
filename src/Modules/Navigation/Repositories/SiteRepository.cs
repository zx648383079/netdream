using NetDream.Shared.Providers;

namespace NetDream.Modules.Navigation.Repositories
{
    public class SiteRepository(NavigationContext db)
    {
        const string BASE_KEY = "search";

        public TagProvider Tag()
        {
            return new TagProvider(db, BASE_KEY);
        }
    }
}
