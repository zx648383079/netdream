using NPoco;
namespace Modules.Navigation.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class PageEntity
    {
        internal const string ND_TABLE_NAME = "search_page";
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        [Column("site_id")]
        public string SiteId { get; set; } = string.Empty;
        public byte Score { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
