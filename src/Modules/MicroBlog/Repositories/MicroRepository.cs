using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.MicroBlog.Repositories
{
    public class MicroRepository(IDatabase db, IClientEnvironment environment)
    {
        const string BASE_KEY = "micro";

        public CommentProvider Comment()
        {
            return new CommentProvider(db, BASE_KEY, environment);
        }
    }
}
