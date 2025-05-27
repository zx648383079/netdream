using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;

namespace NetDream.Modules.OnlineMedia.Repositories
{
    public class TVRepository(MediaContext db, IClientContext client)
    {
        public ActionLogProvider Log()
        {
            return new ActionLogProvider(db, client);
        }

        public TagProvider Tag()
        {
            return new TagProvider(db);
        }
    }
}
