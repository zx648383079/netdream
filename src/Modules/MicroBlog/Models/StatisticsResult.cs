namespace NetDream.Modules.MicroBlog.Models
{
    public class StatisticsResult
    {
        public int PostCount { get; set; }
        public int PostToday { get; set; }

        public int CommentCount { get; set; }
        public int CommentToday { get; set; }

        public int TopicCount { get; set; }
        public int TopicToday { get; set; }
    }
}
