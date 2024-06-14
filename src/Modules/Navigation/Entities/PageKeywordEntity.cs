using NPoco;
namespace Modules.Navigation.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class PageKeywordEntity
    {
        internal const string ND_TABLE_NAME = "search_page_keyword";
        [Column("page_id")]
        public int PageId { get; set; }
        [Column("word_id")]
        public int WordId { get; set; }
        [Column("is_official")]
        public byte IsOfficial { get; set; }
    }
}
