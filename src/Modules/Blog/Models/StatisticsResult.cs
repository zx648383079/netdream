namespace NetDream.Modules.Blog.Models
{
    public class StatisticsResult
    {

        public int TermCount { get; set; }

        public int BlogCount { get; set; }
        public int BlogToday { get; set; }

        public int ViewCount { get; set; }
        public int ViewToday { get; set; }
        public int CommentCount { get; set; }
        public int CommentToday { get; set; }
    }
}
