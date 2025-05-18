namespace NetDream.Modules.TradeTracker.Models
{
    public class PriceListItem
    {
        public string Product { get; set; } = string.Empty;
        public float Price { get; set; }
        public int CreatedAt { get; set; }
    }
}
