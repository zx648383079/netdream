using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class AdEntity
    {
        internal const string ND_TABLE_NAME = "shop_ad";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("position_id")]
        public int PositionId { get; set; }
        public int Type { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        [Column("start_at")]
        public int StartAt { get; set; }
        [Column("end_at")]
        public int EndAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
