﻿using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// 获取简单版用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IUser? Get(int userId);
        /// <summary>
        /// 批量获取简单用户信息
        /// </summary>
        /// <param name="userItems"></param>
        /// <returns></returns>
        public IEnumerable<IUser> Get(params int[] userItems);
        /// <summary>
        /// 根据用户名获取
        /// </summary>
        /// <param name="userItems"></param>
        /// <returns></returns>
        public IEnumerable<IUser> Get(params string[] userItems);
        /// <summary>
        /// 搜索获取用户的id
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="userIds">限制搜索范围</param>
        /// <param name="checkEmpty">true 为先判断 userIds 是否为空</param>
        /// <returns></returns>
        public int[] SearchUserId(string keywords, IList<int>? userIds = null, bool checkEmpty = false);

        public void WithUser(IWithUserModel model);
        public void WithUser(IEnumerable<IWithUserModel> items);
    }
}
