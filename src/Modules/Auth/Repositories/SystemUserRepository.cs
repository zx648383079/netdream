using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Helpers;
using System.Collections.Generic;
using System.Linq;
using NetDream.Shared.Providers;
using NetDream.Shared.Models;
using System;

namespace NetDream.Modules.Auth.Repositories
{
    /// <summary>
    /// 提供全局引用，不应该依赖其他类
    /// </summary>
    /// <param name="db"></param>
    public class SystemUserRepository(AuthContext db): IUserRepository
    {
        public bool Exist(int userId)
        {
            return db.Users.Where(i => i.Id == userId).Any();
        }

        public IUser? Get(int userId)
        {
            return db.Users.Where(i => i.Id == userId).Select(i => new UserSimpleModel()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar
            }).Single();
        }

        public IUser? GetProfile(int userId)
        {
            var model = db.Users.Where(i => i.Id == userId).Select(i => new UserProfileModel()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar
            }).Single();
            if (model is not null)
            {
                model.BulletinCount = db.BulletinUsers.Where(i => i.UserId == userId && i.Status == 0).Count();
            }
            return model;
        }

        public IEnumerable<IUser> Get(params int[] userItems)
        {
            return db.Users.Where(i => userItems.Contains(i.Id)).Select(i => new UserSimpleModel()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar
            }).ToArray();
        }

        /// <summary>
        /// 根据用户名获取
        /// </summary>
        /// <param name="userItems"></param>
        /// <returns></returns>
        public IEnumerable<IUser> Get(params string[] userItems)
        {
            return db.Users.Where(i => userItems.Contains(i.Name)).Select(i => new UserSimpleModel()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar
            }).ToArray();
        }

        public IPage<IUser> Search(string keywords, int page, 
            int[]? items = null, 
            bool itemsIsExclude = true)
        {
            var query = db.Users.Search(keywords, "name");
            if (items is not null)
            {
                if (itemsIsExclude)
                {
                    query = query.Where(i => !items.Contains(i.Id));
                } else
                {
                    query = query.Where(i => items.Contains(i.Id));
                }
            }
            return query.Select(i => new UserSimpleModel()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar
            }).ToPage(page).ConvertTo<UserSimpleModel, IUser>();
        }

        public int[] SearchUserId(string keywords, IList<int>? userIds = null, bool checkEmpty = false)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return userIds is null ? [] : [..userIds];
            }
            if (checkEmpty && (userIds is null || userIds.Count == 0))
            {
                return [];
            }
            return db.Users.Search(keywords, "name")
                .When(userIds is not null && userIds.Count > 0, i => userIds!.Contains(i.Id))
                .Select(i => i.Id).ToArray();
        }

        public void WithUser(IWithUserModel model)
        {
            model.User = Get(model.UserId);
        }
        public void WithUser(IEnumerable<IWithUserModel> items)
        {
            var idItems = items.Select(item => item.UserId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = Get(idItems.ToArray());
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.UserId == it.Id)
                    {
                        item.User = it;
                        break;
                    }
                }
            }
        }
    }
}
