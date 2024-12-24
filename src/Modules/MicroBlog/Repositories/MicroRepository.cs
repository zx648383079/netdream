using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.MicroBlog.Repositories
{
    public class MicroRepository(MicroBlogContext db, IClientContext environment)
    {
        public CommentProvider Comment()
        {
            return new CommentProvider(db, environment);
        }
    }
}
