using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Bot.Entities;
using NetDream.Modules.Bot.Forms;
using NetDream.Modules.Bot.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Bot.Repositories
{
    public class FollowRepository(BotContext db, IClientContext client)
    {
        public IPage<UserListItem> GetList(int bot_id, UserQueryForm form)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return new Page<UserListItem>();
            }
            return db.Users.Where(i => i.BotId == bot_id)
                .When(form.Blacklist > 0, i => i.IsBlack == 1)
                .When(form.Group > 0, i => i.GroupId == form.Group)
                .Search(form.Keywords, "note_name", "nickname")
                .OrderByDescending(i => i.Status)
                .ThenBy(i => i.IsBlack)
                .ThenByDescending(i => i.SubscribeAt)
                .ToPage(form, i => i.SelectAs());
        }

        public IPage<UserListItem> ManageList(int bot_id = 0, UserQueryForm form)
        {
            return db.Users.Where(i => i.BotId == bot_id)
                .When(form.Blacklist > 0, i => i.IsBlack == 1)
                .When(form.Group > 0, i => i.GroupId == form.Group)
                .Search(form.Keywords, "note_name", "nickname")
                .OrderByDescending(i => i.Status)
                .ThenBy(i => i.IsBlack)
                .ThenByDescending(i => i.SubscribeAt)
                .ToPage(form, i => i.SelectAs());
        }

        public IOperationResult Add(int bot_id, string openid, object info)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail("数据错误");
            }
            var model = db.Users.Where(i => i.Openid == openid && i.BotId == bot_id)
                .FirstOrDefault();
            if (model is null)
            {
                model = new()
                {
                    Openid = openid,
                    BotId = bot_id,
                };
            }
            if (info is not null)
            {
               //  model.Set(info);
            }
            else
            {
                model.Status = BotRepository.STATUS_SUBSCRIBED;
                model.SubscribeAt = client.Now;
            }
            db.Users.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Delete(int bot_id, string openid)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail("数据错误");
            }
            db.Users.Where(i => i.Openid == openid && i.BotId == bot_id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, BotRepository.STATUS_UNSUBSCRIBED)
                .SetProperty(i => i.UpdatedAt, client.Now));
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Sync(int bot_id)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail("数据错误");
            }
            new BotRepository().Entry(bot_id)
                .PullUsers(data => {
                    Add(bot_id, data["openid"], data);
                });
            return OperationResult.Ok();
        }

        public ListLabelItem[] SearchFans(int bot_id, QueryForm form)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return [];
            }
            return db.Users.Where(i => i.BotId == bot_id && i.Status == BotRepository.STATUS_SUBSCRIBED)
                .Search(form.Keywords, "nickname", "note_name")
                .Take(form.PerPage)
                .Select(i => new ListLabelItem(i.Id, i.Nickname)).ToArray();
        }

        public IOperationResult Update(int id, object data)
        {
            var model = db.Users.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail("会员不存在");
            }
            if (!AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail("数据错误");
            }
            return OperationResult.Ok();
            // return ModelHelper.UpdateField(model, ["is_black", "note_name", "remark", "group_id"], data);
        }

        public UserLabelItem[] Search(int bot_id, QueryForm form, int[] idItems)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return [];
            }
            if (idItems.Length > 0)
            {
                return db.Users.Where(i => i.BotId == bot_id && idItems.Contains(i.Id))
                    .Search(form.Keywords, "nickname", "note_name").SelectAsLabel().ToArray();
            }
            return db.Users.Where(i => i.BotId == bot_id)
                    .Search(form.Keywords, "nickname", "note_name").Take(form.PerPage)
                    .SelectAsLabel().ToArray();
        }

        public UserGroupEntity[] GroupSearch(int bot_id, QueryForm form, int[] idItems)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return [];
            }
            if (idItems.Length > 0)
            {
                return db.UserGroups.Where(i => i.BotId == bot_id && idItems.Contains(i.Id))
                    .Search(form.Keywords, "name").ToArray();
            }
            return db.UserGroups.Where(i => i.BotId == bot_id)
                    .Search(form.Keywords, "name").Take(form.PerPage).ToArray();
        }

        public IPage<UserGroupEntity> GroupList(int bot_id, QueryForm form)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return new Page<UserGroupEntity>();
            }
            return db.UserGroups.Where(i => i.BotId == bot_id)
                .Search(form.Keywords, "name")
                .ToPage(form);
        }

        public IOperationResult<UserGroupEntity> GroupSave(int bot_id, GroupForm data)
        {
            data.BotId = bot_id;
            UserGroupEntity? model;
            if (data.Id > 0)
            {
                model = db.UserGroups.Where(i => i.BotId == data.BotId && i.Id == data.Id).SingleOrDefault();
                if (model is null)
                {
                    return OperationResult<UserGroupEntity>.Fail("分组不存在");
                }
            }
            else
            {
                model = new()
                {
                    BotId = bot_id
                };
            }
            if (!AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult<UserGroupEntity>.Fail("分组不存在");
            }
            model.Name = data.Name;
            model.TagId = data.TagId;
            db.UserGroups.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult GroupRemove(int bot_id, int id)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail("分组不存在");
            }
            var model = db.UserGroups.Where(i => i.BotId == bot_id && i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("分组不存在");
            }
            db.UserGroups.Remove(model);
            db.SaveChanges();
            return OperationResult.Ok();
        }
    }
}
