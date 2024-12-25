using NetDream.Shared.Providers;

namespace NetDream.Modules.Navigation.Repositories
{
    public class SiteRepository(NavigationContext db)
    {
        public TagProvider Tag()
        {
            return new TagProvider(db);
        }
    }
}
