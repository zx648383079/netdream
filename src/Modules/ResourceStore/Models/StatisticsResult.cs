namespace NetDream.Modules.ResourceStore.Models
{
    public class StatisticsResult
    {
        public int ResourceCount { get; set; }
        public int ResourceToday { get; set; }

        public int DownloadCount { get; set; }
        public int DownloadToday { get; set; }

        public int ViewCount { get; set; }
        public int ViewToday { get; set; }

        public int CommentCount { get; set; }
        public int CommentToday { get; set; }
    }
}
