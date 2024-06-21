namespace NetDream.Shared.Models
{
    public class StatisticsItem(string name, int count)
    {
        public string Name { get; set; } = name;

        public int Count { get; set; } = count;

        public string Icon { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public int Inc { get; set; }

        public StatisticsItem(string name, int count, string unit): this(name, count)
        {
            Unit = unit;
        }
    }
}
