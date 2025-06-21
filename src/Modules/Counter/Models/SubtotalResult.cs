namespace NetDream.Modules.Counter.Models
{
    public class SubtotalResult
    {
        public StageItem[] StageItems { get; set; }
    }

    public class CompareResult
    {
        public StageItem[] Items { get; set; }
        public StageItem[] CompareItems { get; set; }
    }

    public class StageItem
    {
        public int Date { get; set; }
        public int Pv { get; set; }
        public int Uv { get; set; }
    }

    public class DayResult
    {
        public TimeResult Today { get; set; }

        public TimeResult Yesterday { get; set; }
        public TimeResult YesterdayHour { get; set; }

        public TimeResult ExpectToday { get; set; }
    }
}
