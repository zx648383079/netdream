using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CreditLogEntity
    {
        internal const string ND_TABLE_NAME = "user_credit_log";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public int Type { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        public int Credits { get; set; }
        [Column("total_credits")]
        public int TotalCredits { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
