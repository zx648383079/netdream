using NetDream.Core.Providers;
using NPoco;

namespace NetDream.Modules.Book.Repositories
{
    public class BookRepository(IDatabase db)
    {
        const string BASE_KEY = "book";
        public const int CHAPTER_TYPE_FREE_CHAPTER = 0;
        public const int CHAPTER_TYPE_VIP_CHAPTER = 1;
        public const int CHAPTER_TYPE_GROUP = 9; // 卷

        public ActionLogProvider Log()  
        {
            return new ActionLogProvider(db, BASE_KEY);
        }

        public TagProvider Tag() 
        {
            return new TagProvider(db, BASE_KEY);
        }

        public DayLogProvider ClickLog() 
        {
            return new DayLogProvider(db, BASE_KEY);
        }
    }
}
