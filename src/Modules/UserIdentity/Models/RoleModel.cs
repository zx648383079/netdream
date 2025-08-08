using NetDream.Modules.UserIdentity.Entities;

namespace NetDream.Modules.UserIdentity.Models
{
    public class RoleModel : RoleEntity
    {

        public int[] Permissions { get; set; }
    }
}
