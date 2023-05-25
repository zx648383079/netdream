using NPoco;
namespace NetDream.Modules.Document.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class FieldEntity
    {
        internal const string ND_TABLE_NAME = "doc_field";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        [Column("is_required")]
        public int IsRequired { get; set; }
        [Column("default_value")]
        public string DefaultValue { get; set; } = string.Empty;
        public string Mock { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        [Column("api_id")]
        public int ApiId { get; set; }
        public int Kind { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
