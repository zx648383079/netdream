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
        public IUserSource? Get(int userId);
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
        public IUserSource[] Get(params int[] userItems);
        public IDictionary<int, IUserSource> GetDictionary(int[] userItems);
        /// <summary>
        /// 根据用户名获取
        /// </summary>
        /// <param name="userItems"></param>
        /// <returns></returns>
        public IUserSource[] Get(params string[] userItems);
        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="page"></param>
        /// <param name="items">已知的用户</param>
        /// <param name="itemsIsExclude">是排除已知用户还是仅限已知用户</param>
        /// <returns></returns>
        public IPage<IUserSource> Search(IQueryForm form, int[]? items = null, bool itemsIsExclude = true);

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

        /// <summary>
        /// 附加项
        /// </summary>
        /// <param name="user"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Attach(int user, string key, string value);
        /// <summary>
        /// 获取附加项
        /// </summary>
        /// <param name="user"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string? GetAttached(int user, string key);


    }

    public interface IZoneRepository
    {
        /// <summary>
        /// 判断是否是分区
        /// </summary>
        /// <param name="user"></param>
        /// <param name="zone"></param>
        /// <returns></returns>
        public bool IsZone(int user, int zone);
        public int GetZone(int user);
    }
}
