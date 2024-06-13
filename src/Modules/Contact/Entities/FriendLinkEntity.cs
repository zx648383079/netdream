using NPoco;
namespace NetDream.Modules.Contact.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class FriendLinkEntity
    {
        internal const string ND_TABLE_NAME = "cif_friend_link";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Status { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
