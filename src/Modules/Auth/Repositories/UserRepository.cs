using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Modules.Auth.Models;
using NPoco;
using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Helpers;

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

        public int[] SearchUserId(string keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return [];
            }
            var sql = new Sql();
            sql.Select("id").From<UserEntity>(db);
            SearchHelper.Where(sql, "name", keywords);
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
