using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Team.Entities;
using NetDream.Modules.Team.Forms;
using NetDream.Modules.Team.Models;
using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Team.Repositories
{
    public class TeamRepository(TeamContext db, 
        IClientContext client,
        IUserRepository userStore,
        IApplyRepository applyStore,
        IEventBus mediator): ITeamRepository
    {
        public TeamListItem[] All()
        {
            var ids = db.TeamUsers.Where(i => i.UserId == client.UserId)
                .Select(i => i.TeamId)
                .ToArray();
            return GetAny(ids);
        }


        public IListLabelItem[] Get(int[] idItems)
        {
            return GetAny(idItems);
        }

        public TeamListItem[] GetAny(int[] idItems)
        {
            if (idItems.Length == 0)
            {
                return [];
            }
            return db.Teams.Where(i => idItems.Contains(i.Id))
                .SelectAs()
                .ToArray();
        }

        public IOperationResult<TeamModel> Detail(int id)
        {
            if (!Canable(id))
            {
                return OperationResult<TeamModel>.Fail("无权限查看");
            }
            var model = db.Teams.Where(i => i.Id == id).SingleOrDefault()?.CopyTo<TeamModel>();
            if (model == null)
            {
                return OperationResult<TeamModel>.Fail("群不存在");
            }
            model.Users = Users(id, new QueryForm());
            return OperationResult.Ok(model);
        }

        public IPage<TeamUserListItem> Users(int id, QueryForm form)
        {
            var items = db.TeamUsers.Search(form.Keywords, "name")
                .Where(i => i.TeamId == id).ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            return items;
        }

        public IUser[] Users(int team, int[] userItems)
        {
            if (userItems.Length == 0)
            {
                return [];
            }
            return db.TeamUsers.Where(i => i.TeamId == team && userItems.Contains(i.UserId)).SelectAs().ToArray();
        }

        public IPage<TeamListItem> Search(QueryForm form)
        {
            var ids = db.TeamUsers.Where(i => i.UserId == client.UserId)
                .Select(i => i.TeamId)
                .ToArray();
            return db.Teams.When(ids.Length > 0, i => !ids.Contains(i.Id))
                .ToPage(form, i => i.SelectAs());
        }

        public bool Canable(int id)
        {
            return db.TeamUsers.Where(i => i.TeamId == id && i.UserId == client.UserId).Any();
        }

        public bool Manageable(int id)
        {
            return db.Teams.Where(i => i.Id == id && i.UserId == client.UserId).Any();
        }

        public IOperationResult Agree(int user, int team)
        {
            if (team < 1)
            {
                return OperationResult.Fail("错误");
            }
            if (!Manageable(team)) {
                return OperationResult.Fail("无权限处理");
            }
            var exist = db.TeamUsers.Where(i => i.TeamId == team && i.UserId == user).Any();
            if (exist)
            {
                return OperationResult.Fail("已处理过");
            }
            var userModel = userStore.Get(user);
            if (userModel is null)
            {
                applyStore.ReceiveCancel(user, ModuleTargetType.Team, team);
                return OperationResult.Fail("用户不存在");
            }
            db.TeamUsers.Save(new TeamUserEntity()
            {
                TeamId = team,
                UserId = user,
                Name = userModel.Name,
                RoleId = 0,
                Status = 5,
            }, client.Now);
            db.SaveChanges();
            applyStore.Receive(user, ModuleTargetType.Team, team, ReviewStatus.Approved);
            return OperationResult.Ok();
        }

        public IOperationResult Apply(int user, int team, string remark)
        {
            if (team < 1)
            {
                return OperationResult.Fail("错误");
            }
            if (Canable(team)) 
            {
                return OperationResult.Fail("你已加入该群");
            }
            var exist = db.Teams.Where(i => i.Id == team).Any();
            if (!exist)
            {
                return OperationResult.Fail("群不存在");
            }
            applyStore.ReceiveCreate(user, ModuleTargetType.Team, team, remark);
            return OperationResult.Ok();
        }

        public IPage<IApplyListItem> ApplyLog(int id, QueryForm form)
        {
            if (!Manageable(id)) {
                // throw new Exception("无权限处理");
                return new Page<IApplyListItem>();
            }
            return applyStore.ReceiveSearch(ModuleTargetType.Team, id, form);
        }

        /// <summary>
        /// 创建群
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOperationResult<TeamEntity> Create(TeamForm data)
        {
            var model = new TeamEntity()
            {
                Name = data.Name,
                Description = data.Description,
                Logo = data.Logo,
                UserId = client.UserId
            };
            db.Teams.Save(model, client.Now);
            if (db.SaveChanges() == 0)
            {
                return OperationResult<TeamEntity>.Fail("error");
            }
            db.TeamUsers.Save(new TeamUserEntity()
            {
                UserId = model.UserId,
                Name = userStore.Get(model.UserId)!.Name,
                TeamId = model.Id,
                RoleId = 99,
                Status = 5,
            }, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /// <summary>
        /// 解散群
        /// </summary>
        /// <param name="id"></param>
        public IOperationResult Disband(int id)
        {
            var model = db.Teams.Where(i => i.Id == id).Single();
            if (model.UserId != client.UserId)
            {
                return OperationResult.Fail("无权限操作");
            }
            db.Teams.Where(i => i.Id == id).ExecuteDelete();
            db.TeamUsers.Where(i => i.TeamId == id).ExecuteDelete();
            db.SaveChanges();
            mediator.Publish(new DisbandTeam(id, client.Now));
            return OperationResult.Ok();
        }


        public LinkExtraRule[] At(ILinkRuler ruler, string content, int team)
        {
            if (string.IsNullOrWhiteSpace(content) || !content.Contains('@'))
            {
                return [];
            }
            var matches = Regex.Matches(content, @"@(\S+?)\s");
            if (matches is null || matches.Count == 0)
            {
                return [];
            }
            var names = matches.ToDictionary(i => i.Groups[1].Value, i => i.Value);

            var users = db.TeamUsers.Where(i => names.Keys.Contains(i.Name))
                .Select(i => new TeamUserEntity()
                {
                    UserId = i.UserId,
                    Name = i.Name,
                }).ToArray();
            if (users.Length == 0)
            {
                return [];
            }
            var rules = new List<LinkExtraRule>();
            var currentUser = client.UserId;
            var userIds = new List<int>();
            foreach (var user in users)
            {
                if (user.UserId != currentUser)
                {
                    userIds.Add(user.UserId);
                }
                rules.Add(ruler.FormatUser(names[user.Name], user.Id));
            }
            if (userIds.Count > 0)
            {
                var groupModel = db.Teams.Where(i => i.Id == team).Single();
                mediator.Publish(BulletinRequest.Create([.. userIds],
                    $"我在群【{groupModel.Name}】提到了你", "[回复]", [
                        ruler.FormatLink("[回复]", "chat")
                    ], BulletinType.ChatAt));
            }
            return [.. rules];
        }
    }
}
