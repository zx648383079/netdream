using NPoco;
namespace NetDream.Modules.SEO.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class AgreementEntity
    {
        internal const string ND_TABLE_NAME = "seo_agreement";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
