using NPoco;
namespace NetDream.Modules.Document.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ProjectEntity
    {
        internal const string ND_TABLE_NAME = "doc_project";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Cover { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public int Status { get; set; }
        [Column("deleted_at")]
        public int DeletedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
