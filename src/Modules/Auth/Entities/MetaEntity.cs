using NPoco;
namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class UserMetaEntity
    {
        internal const string ND_TABLE_NAME = "user_meta";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
