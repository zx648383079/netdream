using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BookLogEntity
    {
        internal const string ND_TABLE_NAME = "book_log";
        public int Id { get; set; }
        [Column("item_type")]
        public int ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public int Action { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
