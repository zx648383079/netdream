using System.Text.Json.Nodes;

namespace NetDream.Modules.TradeTracker.Forms
{
    public class CrawlForm
    {
        public string From { get; set; }
        public string Name { get; set; }

        public CrawlItemForm[] Items { get; set; }
    }

    public class CrawlItemForm
    {
        public JsonNode? Product { get; set; }

        public string Channel { get; set; }
        public float Price { get; set; }
        public int OrderCount { get; set; }
        public byte Type { get; set; }
        public int CreatedAt { get; set; }
    }
}
