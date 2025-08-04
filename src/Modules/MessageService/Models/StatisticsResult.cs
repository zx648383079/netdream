namespace NetDream.Modules.MessageService.Models
{
    public class StatisticsResult
    {

        public int MessageCount { get; set; }
        public int MessageToday { get; set; }
        public int FailureToday { get; set; }
    }
}
