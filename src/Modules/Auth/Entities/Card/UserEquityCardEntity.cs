using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class UserEquityCardEntity
    {
        internal const string ND_TABLE_NAME = "user_equity_card";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public int Status { get; set; }
        [Column("expired_at")]
        public int ExpiredAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("card_id")]
        public int CardId { get; set; }
        public int Exp { get; set; }
    }
}
