using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Modules.UserAccount.Forms;
using NetDream.Modules.UserAccount.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.UserAccount.Repositories
{
    public class BulletinRepository(UserContext db, 
        IClientContext environment, 
        ISystemBulletin bulletin)
    {
        public const byte TYPE_AT = 7;
        public const byte TYPE_COMMENT = 8;
        public const byte TYPE_AGREE = 6;
        public const byte TYPE_MESSAGE = 96;
        public const byte TYPE_OTHER = 99;
        public const int STATUS_NONE = 0;
        public const int STATUS_READ = 1;  // 已阅读

        public static readonly ContactListItem SystemUser = new()
        {
            Name = "[System]",
            Icon = "S",
            Avatar = "/assets/images/favicon.png",
        };

        public IPage<BulletinUserListItem> GetList(
            QueryForm queries, 
            int status = 0, 
            int user = 0, 
            int lastId = 0)
        {
            var query = db.BulletinUsers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(queries.Keywords))
            {
                var ids = db.Bulletins.Search(queries.Keywords, "title")
                    .Select(i => i.Id)
                    .ToArray();
                if (ids.Length == 0)
                {
                    return new Page<BulletinUserListItem>();
                }
                query = query.Where(i => ids.Contains(i.BulletinId));
            }
            if (user != 0)
            {
                var bulletinId = GetUserBulletin(user);
                if (bulletinId.Length == 0)
                {
                    return new Page<BulletinUserListItem>();
                }
                query = query.Where(i => bulletinId.Contains(i.BulletinId));
            }
            var items = query
                .When(status > 0, i => i.Status == status - 1)
                .When(lastId > 0, i => i.Id < lastId)
                .Where(i => i.UserId == environment.UserId)
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.BulletinId).ToPage(queries);
            var idItems = items.Items.Select(i => i.BulletinId).Distinct().ToArray();
            var bulletinItems = db.Bulletins.Where(i => idItems.Contains(i.Id))
                .Select(i => new BulletinEntity()
                {
                    Id = i.Id,
                    UserId = i.UserId,
                    Title   = i.Title,
                    Content = i.Content,
                    ExtraRule = i.ExtraRule,
                    Type = i.Type,
                })
                .ToDictionary(i => i.Id, i => new BulletinListItem(i));
            var userIds = bulletinItems.Values.Where(i => i.UserId > 0).Select(i => i.UserId).Distinct().ToArray();
            var userItems = db.Users.Where(i => userIds.Contains(i.Id))
                .Select(i => new ContactListItem()
                {
                    Id = i.Id,
                    Avatar = i.Avatar,
                    Name = i.Name,
                }).ToDictionary(i => i.Id, i => i);
            foreach (var item in bulletinItems)
            {
                if (item.Value.UserId == 0)
                {
                    item.Value.User = SystemUser;
                    continue;
                }
                if (userItems.TryGetValue(item.Value.UserId, out var u))
                {
                    item.Value.User = u;
                    continue;
                }
                item.Value.User = new ContactListItem()
                {
                    Id = -9,
                    Name = "[用户已删除]",
                    Icon = "删",
                    Avatar = SystemUser.Avatar,
                };
            }
            return new Page<BulletinUserListItem>(items)
            {
                Items = items.Items.Select(i => {
                    return new BulletinUserListItem(i)
                    {
                        Bulletin = bulletinItems.TryGetValue(i.BulletinId, out var b) ? b : null
                    };
                }).ToArray()
            };
        }

        public ContactListItem[] UserList(int extra = 0)
        {
            var bulletinId = db.BulletinUsers.Where(i => i.UserId == environment.UserId)
                .Select(i => i.BulletinId)
                .Distinct().ToArray();
            var userId = bulletinId.Length == 0 ? [] : db.Bulletins
                .Where(i => bulletinId.Contains(i.Id) && i.UserId > 0)
                .Select(i => i.UserId).Distinct().ToList();
            if (extra > 0 && !userId.Contains(extra) && extra != environment.UserId)
            {
                userId.Add(extra);
            }
            if (userId.Count == 0)
            {
                return [SystemUser];
            }
            var users = db.Users.Where(i => userId.Contains(i.Id))
                .Select(i => new ContactListItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Avatar = i.Avatar,
                }).ToArray();
            return [SystemUser, ..users];
        }

        public IOperationResult<int> Create(BulletinForm data)
        {
            if (data.User < 1 || data.User == environment.UserId)
            {
                return OperationResult.Fail<int>("操作错误");
            }
            var user = db.Users.Where(i => i.Id == data.User)
                .Select(i => new UserEntity()
                {
                    Status = i.Status,
                }).SingleOrDefault();
            if (user is null || user.Status < UserRepository.STATUS_ACTIVE)
            {
                return OperationResult.Fail<int>("操作错误");
            }
            if (RelationshipRepository.Is(db, data.User, environment.UserId, 
                RelationshipRepository.TYPE_BLOCKING))
            {
                return OperationResult.Fail<int>("被对方拉黑了");
            }
            if (!CanSend())
            {
                return OperationResult.Fail<int>("你的操作太频繁了，请五分钟后再试");
            }
            var res = bulletin.Message([data.User], data.Title, data.Content, 
                TYPE_MESSAGE, bulletin.Ruler.FormatEmoji(data.Content));
            if (res > 0)
            {
                return OperationResult.Ok(res);
            }
            return OperationResult.Fail<int>("发送失败");
        }

        public bool CanSend()
        {
            return !db.Bulletins.Where(i => i.UserId == environment.UserId && i.CreatedAt > environment.Now - 300).Any();
        }


        /// <summary>
        /// 取一条消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult<BulletinUserEntity> Read(int id)
        {
            var model = db.BulletinUsers.Where(i => i.UserId == environment.UserId && i.BulletinId == id)
                .SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<BulletinUserEntity>("消息不存在");
            }
            model.Status = STATUS_READ;
            db.BulletinUsers.Save(model, environment.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /// <summary>
        /// 标记全部已读
        /// </summary>
        public void ReadAll()
        {
            db.BulletinUsers.Where(i => i.UserId == environment.UserId && i.Status == 0)
                .ExecuteUpdate(setter =>
                setter.SetProperty(i => i.Status, STATUS_READ)
                .SetProperty(i => i.UpdatedAt, environment.Now));
            db.SaveChanges();
        }

        /// <summary>
        /// 删除一条消息
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        public IOperationResult Remove(int id)
        {
            var model = db.BulletinUsers.Where(i => i.UserId == environment.UserId && i.BulletinId == id)
                .SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("消息不存在");
            }
            db.BulletinUsers.Remove(model);
            if (!db.BulletinUsers.Where(i => i.UserId != environment.UserId && i.BulletinId == id).Any())
            {
                db.Bulletins.Where(i => i.Id == id).ExecuteDelete();
            }
            db.SaveChanges();
            return OperationResult.Ok();
        }

        protected int[] GetUserBulletin(int user)
        {
            if (user < 1)
            {
                user = 0;
            }
            var bulletinId = db.BulletinUsers.Where(i => i.UserId == environment.UserId)
                .Select(i => i.BulletinId)
                .Distinct()
                .ToArray();
            if (bulletinId.Length == 0)
            {
                return [];
            }
            return db.Bulletins.Where(i => bulletinId.Contains(i.Id) && i.UserId == user).Select(i => i.Id).ToArray();
        }

        public IOperationResult RemoveUser(int user)
        {
            if (user < 1)
            {
                return OperationResult.Fail("系统消息无法删除");
            }
            var bulletinId = GetUserBulletin(user);
            if (bulletinId.Length == 0)
            {
                return OperationResult.Ok();
            }
            db.BulletinUsers.Where(i => i.UserId == environment.UserId && bulletinId.Contains(i.BulletinId))
                .ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public static int UnReadCount(UserContext db, int user)
        {
            return db.BulletinUsers.Where(i => i.UserId == user && i.Status == 0).Count();
        }
    }
}
