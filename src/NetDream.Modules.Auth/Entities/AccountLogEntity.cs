using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class AccountLogEntity
    {
        internal const string ND_TABLE_NAME = "user_account_log";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public int Type { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        public int Money { get; set; }
        [Column("total_money")]
        public int TotalMoney { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
