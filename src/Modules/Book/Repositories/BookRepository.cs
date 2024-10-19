using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using NPoco;

namespace NetDream.Modules.Book.Repositories
{
    public class BookRepository(IDatabase db, IClientEnvironment environment)
    {
        const string BASE_KEY = "book";
        public const int CHAPTER_TYPE_FREE_CHAPTER = 0;
        public const int CHAPTER_TYPE_VIP_CHAPTER = 1;
        public const int CHAPTER_TYPE_GROUP = 9; // 卷

        public const string DEFAULT_COVER = "/assets/images/book_default.jpg";
        public const int LOG_TYPE_BOOK = 0;
        public const int LOG_TYPE_LIST = 1;

        public const int LOG_ACTION_CLICK = 0;
        public const int LOG_ACTION_COLLECT = 3;
        public const int LOG_ACTION_AGREE = 1;
        public const int LOG_ACTION_DISAGREE = 2;

        public ActionLogProvider Log()  
        {
            return new ActionLogProvider(db, BASE_KEY, environment);
        }

        public TagProvider Tag() 
        {
            return new TagProvider(db, BASE_KEY);
        }

        public DayLogProvider ClickLog() 
        {
            return new DayLogProvider(db, BASE_KEY, environment);
        }
    }
}
