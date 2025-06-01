namespace NetDream.Shared.Interfaces
{
    public interface IIdentityRepository
    {
        /// <summary>
        /// 判断是否是角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsRole(int user, string role);
        /// <summary>
        /// 判断是否有权限
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool HasPermission(int user, string permission);
    }
}
