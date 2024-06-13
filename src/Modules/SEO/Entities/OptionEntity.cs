using NPoco;
namespace NetDream.Modules.SEO.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class OptionEntity
    {
        internal const string ND_TABLE_NAME = "seo_option";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Visibility { get; set; }
        [Column("default_value")]
        public string DefaultValue { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}
