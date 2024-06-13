using NPoco;
namespace NetDream.Modules.SEO.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class EmojiEntity
    {
        internal const string ND_TABLE_NAME = "seo_emoji";
        public int Id { get; set; }
        [Column("cat_id")]
        public int CatId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
