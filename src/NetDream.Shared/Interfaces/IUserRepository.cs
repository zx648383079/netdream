using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// 判断用户是否有效
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool Exist(int userId);

        /// <summary>
        /// 获取简单版用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IUser? Get(int userId);
        /// <summary>
        /// web 内部调用
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IUserProfile? GetProfile(int userId);


        /// <summary>
        /// 批量获取简单用户信息
        /// </summary>
        /// <param name="userItems"></param>
        /// <returns></returns>
        public IUser[] Get(params int[] userItems);
        public IDictionary<int, IUser> GetDictionary(int[] userItems);
        /// <summary>
        /// 根据用户名获取
        /// </summary>
        /// <param name="userItems"></param>
        /// <returns></returns>
        public IUser[] Get(params string[] userItems);
        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="page"></param>
        /// <param name="items">已知的用户</param>
        /// <param name="itemsIsExclude">是排除已知用户还是仅限已知用户</param>
        /// <returns></returns>
        public IPage<IUser> Search(IQueryForm form, int[]? items = null, bool itemsIsExclude = true);

        /// <summary>
        /// 搜索获取用户的id
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="userIds">限制搜索范围</param>
        /// <param name="checkEmpty">true 为先判断 userIds 是否为空</param>
        /// <returns></returns>
        public int[] SearchUserId(string keywords, int[]? userIds = null, bool checkEmpty = false);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="model"></param>
        public void Include(IWithUserModel model);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="items"></param>
        public void Include(IEnumerable<IWithUserModel> items);

    }
}
