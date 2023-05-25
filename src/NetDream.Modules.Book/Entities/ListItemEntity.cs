using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ListItemEntity
    {
        internal const string ND_TABLE_NAME = "book_list_item";
        public int Id { get; set; }
        [Column("list_id")]
        public int ListId { get; set; }
        [Column("book_id")]
        public int BookId { get; set; }
        public string Remark { get; set; } = string.Empty;
        public int Star { get; set; }
        [Column("agree_count")]
        public int AgreeCount { get; set; }
        [Column("disagree_count")]
        public int DisagreeCount { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
