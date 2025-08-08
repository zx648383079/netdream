using NetDream.Modules.UserAccount.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NetDream.Modules.UserAccount.Repositories
{
    /// <summary>
    /// 提供全局引用，不应该依赖其他类
    /// </summary>
    /// <param name="db"></param>
    public class SystemUserRepository(UserContext db): IUserRepository
    {
        public bool Exist(int userId)
        {
            if (userId <= 0)
            {
                return false;
            }
            return db.Users.Where(i => i.Id == userId).Any();
        }

        public IUserSource? Get(int userId)
        {

            if (userId <= 0)
            {
                return null;
            }
            return db.Users.Where(i => i.Id == userId).Select(i => new UserLabelItem()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar
            }).SingleOrDefault();
        }

        public IUserProfile? GetProfile(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }
            var model = db.Users.Where(i => i.Id == userId).Select(i => new UserProfileModel()
            {
                Id = i.Id,
                Name = i.Name,
                Mobile = i.Mobile,
                Email = i.Email,
                Sex = i.Sex,
                Birthday = i.Birthday,
                Avatar = i.Avatar,
                CreatedAt = i.CreatedAt,
            }).SingleOrDefault();
            return model;
        }

        public IUserSource[] Get(params int[] userItems)
        {
            userItems = [.. userItems.Where(i => i > 0).Distinct()];
            if (userItems.Length == 0)
            {
                return [];
            }
            
            return db.Users.Where(i => userItems.Contains(i.Id))
                .Select(i => new UserLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Avatar = i.Avatar
                })
                .ToArray();
        }

        public IDictionary<int, IUserSource> GetDictionary(int[] userItems)
        {
            return Get(userItems).ToDictionary(i => i.Id);
        }

        /// <summary>
        /// 根据用户名获取
        /// </summary>
        /// <param name="userItems"></param>
        /// <returns></returns>
        public IUserSource[] Get(params string[] userItems)
        {
            userItems = [.. userItems.Where(i => !string.IsNullOrWhiteSpace(i)).Distinct()];
            if (userItems.Length == 0)
            {
                return [];
            }
            return db.Users.Where(i => userItems.Contains(i.Name))
                .Select(i => new UserLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Avatar = i.Avatar
                }).ToArray();
        }

        

        public IPage<IUserSource> Search(IQueryForm form, 
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
            return query.Select(i => new UserLabelItem()
            {
                Id = i.Id,
                Name = i.Name,
                Avatar = i.Avatar
            }).ToPage(form).ConvertTo<UserLabelItem, IUserSource>();
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

        public void Attach(int user, string key, string value)
        {
            var data = db.Metas.Where(i => i.ItemId == user && i.Name == key).FirstOrDefault();
            if (data != null)
            {
                data.Content = value;
            } else
            {
                data = new Shared.Providers.Entities.MetaEntity()
                {
                    ItemId = user,
                    Name = key,
                    Content = value
                };
            }
            db.Metas.Save(data);
            db.SaveChanges();
        }

        public string? GetAttached(int user, string key)
        {
            return db.Metas.Where(i => i.ItemId == user && i.Name == key).Value(i => i.Content);
        }

        public bool IsRole(int user, string role)
        {
            // TODO
            return false;
        }

        public bool HasPermission(int user, string permission)
        {
            // TODO
            return false;
        }
    }
}
