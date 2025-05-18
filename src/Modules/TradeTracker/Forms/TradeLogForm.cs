namespace NetDream.Modules.TradeTracker.Forms
{
    public class TradeLogForm
    {
        public int ProductId { get; set; }
        public int ChannelId { get; set; }
        public byte Type { get; set; }
        public float Price { get; set; }
        public int OrderCount { get; set; }
        public int CreatedAt { get; set; }
    }
}
