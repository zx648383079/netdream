namespace NetDream.Modules.Contact.Models
{
    public class StatisticsResult
    {
        public int FeedbackNew { get; set; }
        public int FeedbackCount { get; set; }

        public int ReportNew { get; set; }
        public int ReportCount { get; set; }
        public int LinkCount { get; set; }
        public int LinkToday { get; set; }

        public int SubscribeCount { get; set; }
        public int SubscribeToday { get; set; }
    }
}
