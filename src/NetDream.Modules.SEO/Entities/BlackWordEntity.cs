using NPoco;
namespace NetDream.Modules.SEO.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BlackWordEntity
    {
        internal const string ND_TABLE_NAME = "seo_black_word";
        public int Id { get; set; }
        public string Words { get; set; } = string.Empty;
        [Column("replace_words")]
        public string ReplaceWords { get; set; } = string.Empty;
    }
}
