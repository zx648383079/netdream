using NetDream.Modules.Auth.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (userId <= 0)
            {
                return false;
            }
            return db.Users.Where(i => i.Id == userId).Any();
        }

        public IUser? Get(int userId)
        {

            if (userId <= 0)
            {
                return null;
            }
            return db.Users.Where(i => i.Id == userId).Select(i => new UserListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar
            }).SingleOrDefault();
        }

        public IUser? GetProfile(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }
            var model = db.Users.Where(i => i.Id == userId).Select(i => new UserProfileModel()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar
            }).SingleOrDefault();
            if (model is not null)
            {
                model.MetaItems = new()
                {
                    {"bulletin_count", BulletinRepository.UnReadCount(db, userId) }
                };
            }
            return model;
        }

        public IUser[] Get(params int[] userItems)
        {
            userItems = [.. userItems.Where(i => i > 0).Distinct()];
            if (userItems.Length == 0)
            {
                return [];
            }
            
            return db.Users.Where(i => userItems.Contains(i.Id))
                .Select(i => new UserListItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Avatar = i.Avatar
                })
                .ToArray();
        }

        public IDictionary<int, IUser> GetDictionary(int[] userItems)
        {
            return Get(userItems).ToDictionary(i => i.Id);
        }

        /// <summary>
        /// 根据用户名获取
        /// </summary>
        /// <param name="userItems"></param>
        /// <returns></returns>
        public IUser[] Get(params string[] userItems)
        {
            userItems = [.. userItems.Where(i => !string.IsNullOrWhiteSpace(i)).Distinct()];
            if (userItems.Length == 0)
            {
                return [];
            }
            return db.Users.Where(i => userItems.Contains(i.Name))
                .Select(i => new UserListItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Avatar = i.Avatar
                }).ToArray();
        }

        public IPage<IUser> Search(IQueryForm form, 
            int[]? items = null, 
            bool itemsIsExclude = true)
        {
            var query = db.Users.Search(form.Keywords, "name");
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
            return query.Select(i => new UserListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar
            }).ToPage(form).ConvertTo<UserListItem, IUser>();
        }

        public int[] SearchUserId(string keywords, int[]? userIds = null, bool checkEmpty = false)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return userIds is null ? [] : [..userIds];
            }
            if (checkEmpty && (userIds is null || userIds.Length == 0))
            {
                return [];
            }
            return db.Users.Search(keywords, "name")
                .When(userIds is not null && userIds.Length > 0, i => userIds!.Contains(i.Id))
                .Select(i => i.Id).ToArray();
        }

        public void Include(IWithUserModel model)
        {
            model.User = Get(model.UserId);
        }
        public void Include(IEnumerable<IWithUserModel> items)
        {
            var idItems = items.Select(item => item.UserId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = GetDictionary(idItems);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.UserId > 0 && data.TryGetValue(item.UserId, out var user))
                {
                    item.User = user;
                }
            }
        }
    }
}
