using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CommentEntity
    {
        internal const string ND_TABLE_NAME = "shop_comment";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("item_type")]
        public int ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Rank { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
