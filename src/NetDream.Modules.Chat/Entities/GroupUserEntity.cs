using NPoco;
namespace NetDream.Modules.Chat.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class GroupUserEntity
    {
        internal const string ND_TABLE_NAME = "chat_group_user";
        public int Id { get; set; }
        [Column("group_id")]
        public int GroupId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("role_id")]
        public int RoleId { get; set; }
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
