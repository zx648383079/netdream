using NPoco;
namespace Modules.Navigation.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CollectEntity
    {
        internal const string ND_TABLE_NAME = "search_collect";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        [Column("group_id")]
        public int GroupId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public byte Position { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
