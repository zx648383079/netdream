using NPoco;
namespace Modules.Navigation.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class SiteEntity
    {
        internal const string ND_TABLE_NAME = "search_site";
        public int Id { get; set; }
        public string Schema { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column("cat_id")]
        public int CatId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("top_type")]
        public byte TopType { get; set; }
        public byte Score { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
