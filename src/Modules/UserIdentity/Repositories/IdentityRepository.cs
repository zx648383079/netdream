using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.UserIdentity.Repositories
{
    public class IdentityRepository(IdentityContext db) : IIdentityRepository
    {
        /// <summary>
        /// 判断是否是角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsRole(int user, string role)
        {
            if (user <= 0)
            {
                return false;
            }
            var roleId = db.Roles.Where(i => i.Name == role).Value(i => i.Id);
            if (roleId == 0)
            {
                return false;
            }
            return db.UserRoles.Where(i => i.UserId == user && (i.RoleId == roleId || i.RoleId == 1)).Any();
        }
        /// <summary>
        /// 判断是否有权限
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool HasPermission(int user, string permission)
        {
            if (user <= 0)
            {
                return false;
            }
            if (user <= 0)
            {
                return false;
            }
            var permissionId = db.Permissions.Where(i => i.Name == permission).Value(i => i.Id);
            if (permissionId == 0)
            {
                return false;
            }
            var roleIds = db.RolePermissions.Where(i => i.PermissionId == permissionId).Select(i => i.RoleId).ToList();
            roleIds.Add(1);
            return db.UserRoles.Where(i => i.UserId == user && roleIds.Contains(i.RoleId)).Any();
        }
    }
}
