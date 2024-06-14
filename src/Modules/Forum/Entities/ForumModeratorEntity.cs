using NPoco;
namespace Modules.Forum.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ForumModeratorEntity
    {
        internal const string ND_TABLE_NAME = "bbs_forum_moderator";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("forum_id")]
        public int ForumId { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
    }
}
