using NPoco;
namespace NetDream.Modules.SEO.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class EmojiCategoryEntity
    {
        internal const string ND_TABLE_NAME = "seo_emoji_category";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}
