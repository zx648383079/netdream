using NPoco;
namespace NetDream.Modules.Document.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ProjectVersionEntity
    {
        internal const string ND_TABLE_NAME = "doc_project_version";
        public int Id { get; set; }
        [Column("project_id")]
        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
