using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Modules.Auth.Models;
using NPoco;
using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Auth.Repositories
{
    /// <summary>
    /// 提供全局引用，不应该依赖其他类
    /// </summary>
    /// <param name="db"></param>
    public class SystemUserRepository(IDatabase db): IUserRepository
    {
        public bool Exist(int userId)
        {
            return db.FindCount<int, UserSimpleModel>("id=@0", userId) > 0;
        }

        public IUser? Get(int userId)
        {
            return db.FindFirst<UserSimpleModel>("id,name,avatar", "id=@0", userId);
        }

        public IUser? GetProfile(int userId)
        {
            var model = db.FindFirst<UserProfileModel>("id,name,avatar", "id=@0", userId);
            if (model is not null)
            {
                model.BulletinCount = db.FindCount<BulletinUserEntity>("user_id=@0 and status=0", userId);
            }
            return model;
        }

        public IEnumerable<IUser> Get(params int[] userItems)
        {
            var sql = new Sql();
            sql.Select("id,name,avatar");
            sql.From<UserSimpleModel>(db);
            sql.WhereIn("id", userItems);
            return db.Fetch<UserSimpleModel>(sql);
        }

        /// <summary>
        /// 根据用户名获取
        /// </summary>
        /// <param name="userItems"></param>
        /// <returns></returns>
        public IEnumerable<IUser> Get(params string[] userItems)
        {
            var sql = new Sql();
            sql.Select("id,name,avatar");
            sql.From<UserSimpleModel>(db);
            sql.WhereIn("name", userItems);
            return db.Fetch<UserSimpleModel>(sql);
        }

        public Page<IUser> Search(string keywords, int page, 
            int[]? items = null, 
            bool itemsIsExclude = true)
        {
            var sql = new Sql();
            sql.Select("id,name,avatar").From<UserEntity>(db);
            SearchHelper.Where(sql, "name", keywords);
            if (items is not null)
            {
                if (itemsIsExclude)
                {
                    sql.WhereNotIn("id", items);
                } else
                {
                    sql.WhereIn("id", items);
                }
            }
            var res = db.Page<UserSimpleModel>(page, 20, sql);
            return new Page<IUser>()
            {
                CurrentPage = res.CurrentPage,
                ItemsPerPage = res.ItemsPerPage,
                TotalItems = res.TotalItems,
                TotalPages = res.TotalPages,
                Items = res.Items.Select(x => (IUser)x).ToList()
            };
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
            var sql = new Sql();
            sql.Select("id").From<UserEntity>(db);
            SearchHelper.Where(sql, "name", keywords);
            if (userIds is not null && userIds.Count > 0)
            {
                sql.WhereIn("id", [..userIds]);
            }
            return [.. db.Pluck<int>(sql)];
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
