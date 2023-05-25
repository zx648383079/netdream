using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ActivityTimeEntity
    {
        internal const string ND_TABLE_NAME = "shop_activity_time";
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        [Column("start_at")]
        public TimeSpan StartAt { get; set; }
        [Column("end_at")]
        public TimeSpan EndAt { get; set; }
    }
}
