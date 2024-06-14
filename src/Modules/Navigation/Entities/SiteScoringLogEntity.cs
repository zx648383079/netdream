using NPoco;
namespace Modules.Navigation.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class SiteScoringLogEntity
    {
        internal const string ND_TABLE_NAME = "search_site_scoring_log";
        public int Id { get; set; }
        [Column("site_id")]
        public int SiteId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public byte Score { get; set; }
        [Column("last_score")]
        public byte LastScore { get; set; }
        [Column("change_reason")]
        public string ChangeReason { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
