using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CommentImageEntity
    {
        internal const string ND_TABLE_NAME = "shop_comment_image";
        public int Id { get; set; }
        [Column("comment_id")]
        public int CommentId { get; set; }
        public string Image { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
