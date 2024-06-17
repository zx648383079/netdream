using NPoco;

namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class UserRoleEntity
    {
        internal const string ND_TABLE_NAME = "rbac_user_role";
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

    }
}
