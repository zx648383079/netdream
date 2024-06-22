using NPoco;

namespace NetDream.Shared.Providers.Models
{
    public class LogCount
    {
        public int Count { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
    }
}
