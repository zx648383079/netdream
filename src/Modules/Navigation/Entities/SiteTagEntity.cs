using NPoco;
namespace Modules.Navigation.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class SiteTagEntity
    {
        internal const string ND_TABLE_NAME = "search_site_tag";
        [Column("tag_id")]
        public int TagId { get; set; }
        [Column("site_id")]
        public int SiteId { get; set; }
    }
}
