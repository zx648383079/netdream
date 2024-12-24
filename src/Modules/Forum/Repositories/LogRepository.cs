using NetDream.Shared.Repositories;

namespace NetDream.Modules.Forum.Repositories
{
    public class LogRepository(ForumContext db) : ActionRepository(db)
    {

        public const byte TYPE_FORUM = 0;
        public const byte TYPE_THREAD = 1;
        public const byte TYPE_POST = 2;
        public const byte ACTION_EDIT = 1;
        public const byte ACTION_DELETE = 2;
        public const byte ACTION_STATUS = 3;

        public const byte TYPE_THREAD_POST = 1;
        public const byte ACTION_COLLECT = 0;
        public const byte ACTION_AGREE = 1;
        public const byte ACTION_DISAGREE = 2;
        public const byte ACTION_BUY = 3;
        public const byte ACTION_VOTE = 4;
        public const byte ACTION_DOWNLOAD = 5;
        public const byte ACTION_REWARD = 6; // 打赏
    }
}
