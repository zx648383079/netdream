using NPoco;
namespace NetDream.Modules.CMS.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class SiteEntity
    {
        internal const string ND_TABLE_NAME = "cms_site";
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        [Column("match_type")]
        public byte MatchType { get; set; }
        [Column("match_rule")]
        public string MatchRule { get; set; } = string.Empty;
        [Column("is_default")]
        public byte IsDefault { get; set; }
        public byte Status { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Options { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
