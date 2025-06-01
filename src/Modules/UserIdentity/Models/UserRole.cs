using NetDream.Modules.UserIdentity.Entities;

namespace NetDream.Modules.UserIdentity.Models
{
    public class UserRole
    {
        public RoleEntity? Role { get; set; }

        public string[] Roles { get; set; } = [];
        public string[] Permissions { get; set; } = [];
    }
}
