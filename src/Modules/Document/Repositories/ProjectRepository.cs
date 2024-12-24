using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;

namespace NetDream.Modules.Document.Repositories
{
    public class ProjectRepository(DocumentContext db, IClientContext environment)
    {
        public CommentProvider Comment()
        {
            return new CommentProvider(db, environment);
        }
    }
}
