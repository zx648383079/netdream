using NPoco;
namespace NetDream.Modules.CMS.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ModelEntity
    {
        internal const string ND_TABLE_NAME = "cms_model";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Table { get; set; } = string.Empty;
        public byte Type { get; set; }
        public byte Position { get; set; }
        [Column("child_model")]
        public int ChildModel { get; set; }
        [Column("category_template")]
        public string CategoryTemplate { get; set; } = string.Empty;
        [Column("list_template")]
        public string ListTemplate { get; set; } = string.Empty;
        [Column("show_template")]
        public string ShowTemplate { get; set; } = string.Empty;
        public string Setting { get; set; } = string.Empty;
    }
}
