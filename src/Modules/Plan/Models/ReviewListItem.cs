namespace NetDream.Modules.Plan.Models
{
    public class ReviewListItem
    {
        public string Day { get; set; }

        public string Week { get; set; }
        public int Amount { get; set; }
        public int CompleteAmount { get; set; }
        public int SuccessAmount { get; set; }
        public int PauseAmount { get; set; }
        public int FailureAmount { get; set; }
        public int TotalTime { get; set; }
        public int ValidTime { get; set; }
    }
}
