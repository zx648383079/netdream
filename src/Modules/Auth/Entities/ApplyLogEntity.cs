using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ApplyLogEntity
    {
        internal const string ND_TABLE_NAME = "user_apply_log";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public int Type { get; set; }
        public int Money { get; set; }
        public string Remark { get; set; } = string.Empty;
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
