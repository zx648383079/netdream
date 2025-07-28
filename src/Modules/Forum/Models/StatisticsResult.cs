namespace NetDream.Modules.Forum.Models
{
    public class StatisticsResult
    {
        public int ForumCount { get; internal set; }
        public int ThreadCount { get; internal set; }
        public int ThreadToday { get; internal set; }
        public int PostCount { get; internal set; }
        public int PostToday { get; internal set; }
        public int ViewCount { get; internal set; }
        public int ViewToday { get; internal set; }
    }
}
