using NPoco;
namespace NetDream.Modules.CMS.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class LinkageEntity
    {
        internal const string ND_TABLE_NAME = "cms_linkage";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}
