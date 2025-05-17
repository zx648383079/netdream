namespace NetDream.Modules.TradeTracker.Entities
{
    public class TradeLogEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ChannelId { get; set; }
        public byte Type { get; set; }
        public decimal Price { get; set; }
        public int CreatedAt { get; set; }
    }
}
