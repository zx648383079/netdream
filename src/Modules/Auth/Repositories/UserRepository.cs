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
    public class UserRepository(IDatabase db): IUserRepository
    {
        public const int STATUS_DELETED = 0; // 已删除
        public const int STATUS_FROZEN = 2; // 账户已冻结
        public const int STATUS_UN_CONFIRM = 9; // 邮箱注册，未确认邮箱
        public const int STATUS_ACTIVE = 10; // 账户正常
        public const int STATUS_ACTIVE_VERIFIED = 15; // 账户正常&实名认证了

        public const int SEX_MALE = 1; // 性别男
        public const int SEX_FEMALE = 2; //性别女

        public bool Exist(int userId)
        {
            return db.FindCount<int, UserSimpleModel>("id=@0", userId) > 0;
        }

        public IUser? Get(int userId)
        {
            return db.FindFirst<UserSimpleModel>("id,name,avatar", "id=@0", userId);
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
