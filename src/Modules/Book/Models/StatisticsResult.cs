namespace NetDream.Modules.Book.Models
{
    public class StatisticsResult
    {
        public int CategoryCount { get; internal set; }
        public int BookCount { get; internal set; }
        public int BookToday { get; internal set; }
        public int ChapterCount { get; internal set; }
        public int ChapterToday { get; internal set; }
        public int WordCount { get; internal set; }
        public int WordToday { get; internal set; }
        public int AuthorCount { get; internal set; }
        public int ListCount { get; internal set; }
        public int ListToday { get; internal set; }
        public int ViewCount { get; internal set; }
        public int ViewToday { get; internal set; }
    }
}
