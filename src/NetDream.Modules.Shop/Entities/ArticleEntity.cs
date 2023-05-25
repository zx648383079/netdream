using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ArticleEntity
    {
        internal const string ND_TABLE_NAME = "shop_article";
        public int Id { get; set; }
        [Column("cat_id")]
        public int CatId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
