using NetDream.Modules.Blog.Entities;
using NetDream.Shared.Repositories;
using NPoco;

namespace NetDream.Modules.Blog.Repositories
{
    public class LogRepository(BlogContext db) : ActionRepository<LogEntity>(db)
    {
        public const byte TYPE_BLOG = 0;
        public const byte TYPE_COMMENT = 1;

        public const byte ACTION_RECOMMEND = 0;
        public const byte ACTION_AGREE = 1;
        public const byte ACTION_DISAGREE = 2;

        public const byte ACTION_REAL_RULE = 3; // 是否能阅读
    }
}
