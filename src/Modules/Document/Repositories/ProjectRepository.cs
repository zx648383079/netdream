using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Document.Repositories
{
    public class ProjectRepository(IDatabase db, IClientContext environment)
    {
        const string BASE_KEY = "doc";
        public CommentProvider Comment()
        {
            return new CommentProvider(db, BASE_KEY, environment);
        }
    }
}
