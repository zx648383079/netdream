using NPoco;
namespace NetDream.Modules.Document.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class PageEntity
    {
        internal const string ND_TABLE_NAME = "doc_page";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Column("version_id")]
        public int VersionId { get; set; }
        [Column("parent_id")]
        public int ParentId { get; set; }
        public int Type { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
