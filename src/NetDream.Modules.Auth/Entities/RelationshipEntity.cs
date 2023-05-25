using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class RelationshipEntity
    {
        internal const string ND_TABLE_NAME = "user_relationship";
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("link_id")]
        public int LinkId { get; set; }
        public int Type { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
