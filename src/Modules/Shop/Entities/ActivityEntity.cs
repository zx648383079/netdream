using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ActivityEntity
    {
        internal const string ND_TABLE_NAME = "shop_activity";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Type { get; set; }
        [Column("scope_type")]
        public int ScopeType { get; set; }
        public string Scope { get; set; } = string.Empty;
        public string Configure { get; set; } = string.Empty;
        public int Status { get; set; }
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
