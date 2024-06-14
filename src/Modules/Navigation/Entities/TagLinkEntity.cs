using NPoco;
namespace Modules.Navigation.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class TagLinkEntity
    {
        internal const string ND_TABLE_NAME = "search_tag_link";
        [Column("tag_id")]
        public int TagId { get; set; }
        [Column("target_id")]
        public int TargetId { get; set; }
    }
}
