using NPoco;
namespace NetDream.Modules.CMS.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class GroupEntity
    {
        internal const string ND_TABLE_NAME = "cms_group";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
