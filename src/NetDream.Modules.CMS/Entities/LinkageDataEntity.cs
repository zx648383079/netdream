using NPoco;
namespace NetDream.Modules.CMS.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class LinkageDataEntity
    {
        internal const string ND_TABLE_NAME = "cms_linkage_data";
        public int Id { get; set; }
        [Column("linkage_id")]
        public int LinkageId { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        public byte Position { get; set; }
        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
