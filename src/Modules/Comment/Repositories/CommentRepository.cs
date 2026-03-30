using MediatR;
using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Comment.Entities;
using NetDream.Modules.Comment.Forms;
using NetDream.Modules.Comment.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Notifications;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Comment.Repositories
{
    public class CommentRepository(CommentContext db, 
        IUserRepository userStore,
        IInteractRepository interact,
        IClientContext client,
        IGlobeOption option,
        ILinkRuler ruler,
        IMediator mediator): ICommentRepository
    {
        public IPage<CommentListItem> GetList(CommentQueryForm form, bool isHot = false)
        {
            SearchHelper.CheckSortOrder(form, ["created_at", "id", "agree_count"]);
            var query = db.Comments.Where(i => i.Status == (byte)ReviewStatus.Approved 
            && i.ItemType == form.TargetType && i.ItemId == form.TargetId
            && i.ParentId == form.Parent);
            if (isHot)
            {
                query = query.Where(i => i.AgreeCount > 0)
                    .OrderByDescending(i => i.AgreeCount);
            } else
            {
                query = query.OrderBy(form);
            }
            var items = query.ToPage(form);
            userStore.Include(items.Items);
            if (form.Parent == 0)
            {
                IncludeReply(items.Items);
            }
            return items;
        }

        private void IncludeReply(IEnumerable<CommentListItem> items)
        {
            var idItems = items.Select(item => item.Id);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Comments.Where(i => idItems.Contains(i.ParentId)).ToArray();
            if (!data.Any())
            {
                return;
            }
            userStore.Include(data);
            foreach (var item in items)
            {
                //item.Replies = [..data.Where(i => i.ParentId == item.Id)];
            }
        }

        public IOperationResult<CommentEntity> Create(CommentForm data)
        {
            var model = new CommentEntity()
            {
                Content = data.Content,
            };
            if (client.UserId > 0)
            {
                model.UserId = client.UserId;
                model.GuestName = userStore.Get(client.UserId).Name;
            }
            if (data.Parent > 0)
            {
                var parent = db.Comments.Where(i => i.Id == data.Parent).Single();
                if (parent.ParentId > 0)
                {
                    parent = db.Comments.Where(i => i.Id == parent.ParentId).Single();
                }
                model.ParentId = parent.Id;
            }
            var last = db.Comments.Where(i => i.ItemId == data.TargetId && i.ItemType == data.TargetType && i.ParentId == model.ParentId)
                .OrderByDescending(i => i.Position).Single();
            model.Position = last is null ? 1 : (last.Position + 1);
            model.Status = (byte)ReviewStatus.None;
            db.Comments.Save(model);
            db.SaveChanges();
            LinkExtraRule[] extraRules = [..
                At(data.Content, model.ItemId, model.ParentId),
                ..ruler.FormatEmoji(data.Content)
            ];
            if (extraRules.Length != 0)
            {
                model.ExtraRule = JsonSerializer.Serialize(extraRules);
                db.Comments.Save(model);
                db.SaveChanges();
            }
            return OperationResult.Ok(model);
        }

        public LinkExtraRule[] At(string content, int targetId, int commentId)
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
            var names = new Dictionary<string, string>();
            var commentPosition = new Dictionary<int, string>();
            foreach (Match match in matches)
            {
                if (Regex.IsMatch(match.Groups[1].Value, @"^\d+#"))
                {
                    commentPosition[int.Parse(match.Groups[1].Value[0..-1])] = match.Value;
                    continue;
                }
                names[match.Groups[1].Value] = match.Value;
            }
            return [..AtUser(names, targetId), ..AtPosition(commentPosition, commentId)];
        }

        protected IEnumerable<LinkExtraRule> AtPosition(Dictionary<int, string> items, 
            int commentId)
        {
            if (commentId < 1)
            {
                return [];
            }
            var commentItems = db.Comments.Where(i => i.ParentId == commentId && items.Keys.Contains(i.Position))
                .ToArray();
            if (commentItems is null || commentItems.Length == 0)
            {
                return [];
            }
            return commentItems.Select(item => {
                return ruler.FormatId(items[item.Position], "comment-" + item.Id);
            });
        }

        protected IEnumerable<LinkExtraRule> AtUser(Dictionary<string, string> names, int targetId)
        {
            if (names.Count == 0)
            {
                return [];
            }
            var users = userStore.Get(names.Keys.ToArray());
            if (!users.Any())
            {
                return [];
            }
            var rules = new List<LinkExtraRule>();
            var currentUser = client.UserId;
            var userIds = new List<int>();
            foreach (var user in users)
            {
                if (user.Id != currentUser)
                {
                    userIds.Add(user.Id);
                }
                rules.Add(
                    ruler.FormatUser(names[user.Name], user.Id));
            }
            if (targetId < 1 || userIds.Count == 0)
            {
                return rules;
            }
            return rules;
        }

        public CommentListItem[] GetHot(int targetId, int limit = 4)
        {
            var items = db.Comments.Where(i => i.ItemId == targetId && i.ParentId == 0 && i.AgreeCount > 0)
                .OrderByDescending(i => i.AgreeCount)
                .Take(limit).ToArray();
            userStore.Include(items);
            return items;
        }

        /// <summary>
        /// 用于后台管理
        /// </summary>
        /// <param name="blog"></param>
        /// <param name="keywords"></param>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IPage<CommentListItem> ManageList(CommentQueryForm form)
        {
            var items = db.Comments.When(form.TargetId > 0, i => i.ItemId == form.TargetId && i.ItemType == form.TargetType)
                .Search(form.Keywords, "content")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
            userStore.Include(items.Items);
            return items;
        }

        public void ManageRemove(int id)
        {
            db.Comments.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        /// <summary>
        /// 前台删除，博主或发表人
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult RemoveSelf(int id)
        {
            var model = db.Comments.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("评论删除失败");
            }
            if (model.UserId > 0 && model.UserId == client.UserId)
            {
                db.Comments.Remove(model);
                db.SaveChanges();
                return OperationResult.Ok();
            }
            db.Comments.Remove(model);
            db.SaveChanges();
            return OperationResult.Ok();
        }


        public CommentListItem[] NewList()
        {
            var items = db.Comments.Where(i => i.Status == (byte)ReviewStatus.Approved)
                .OrderByDescending(i => i.CreatedAt)
                .Take(4).ToArray();
            return items;
        }

        public IOperationResult Report(int id)
        {
            var model = db.Comments.Where(i => i.Id == id).Single();
            mediator.Publish(ReportRequest.Create(client, ModuleTargetType.Comment, id,
                string.Format("“{0}”", model.Content), "举报博客评论"));
            return OperationResult.Ok();
        }

        public IOperationResult<AgreeResult> Disagree(int id)
        {
            var model = db.Comments.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult<AgreeResult>.Fail("评论不存在");
            }
            var res = interact.Update(client.UserId, ModuleTargetType.Comment, id,
                InteractType.Disagree, 
                [InteractType.Disagree, InteractType.Agree]);
            var data = new AgreeResult()
            {
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            switch(res)
            {
                case RecordToggleType.Updated:
                    data.AgreeCount--;
                    data.DisagreeCount++;
                    data.AgreeType = AgreeType.Disagree;
                    break;
                case RecordToggleType.Added:
                    data.DisagreeCount++;
                    data.AgreeType = AgreeType.Disagree;
                    break;
                default:
                    data.DisagreeCount--;
                    data.AgreeType = AgreeType.None;
                    break;

            }
            db.Comments.Where(i => i.Id == model.Id)
                  .ExecuteUpdate(setters => setters
                  .SetProperty(i => i.AgreeCount, data.AgreeCount)
                  .SetProperty(i => i.DisagreeCount, data.DisagreeCount));
            return OperationResult.Ok(data);
        }

        public IOperationResult<AgreeResult> Agree(int id)
        {
            var model = db.Comments.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult<AgreeResult>.Fail("评论不存在");
            }
            var res = interact.Update(client.UserId, ModuleTargetType.Comment, id,
                InteractType.Agree,
                [InteractType.Disagree, InteractType.Agree]);
            var data = new AgreeResult()
            {
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            switch (res)
            {
                case RecordToggleType.Updated:
                    data.AgreeCount++;
                    data.DisagreeCount--;
                    data.AgreeType = AgreeType.Agree;
                    break;
                case RecordToggleType.Added:
                    data.AgreeCount++;
                    data.AgreeType = AgreeType.Agree;
                    break;
                default:
                    data.AgreeCount--;
                    data.AgreeType = AgreeType.None;
                    break;

            }
            db.Comments.Where(i => i.Id == model.Id)
                .ExecuteUpdate(setters => setters
                .SetProperty(i => i.AgreeCount, data.AgreeCount)
                .SetProperty(i => i.DisagreeCount, data.DisagreeCount));
            return OperationResult.Ok(data);
        }

        public IOperationResult<CommentEntity> ManageToggle(int id)
        {
            var model = db.Comments.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<CommentEntity>.Fail("数据错误");
            }
            model.Status = (byte)(model.Status == (byte)ReviewStatus.Approved ? ReviewStatus.Seen : ReviewStatus.Approved);
            db.Comments.Update(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /// <summary>
        /// 获取用户的最后使用信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IOperationResult<GuestUser> LastCommentator(string email)
        {
            var data = db.Comments.Where(i => i.GuestEmail == email)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => new GuestUser()
                {
                    Name = i.GuestName,
                    Email = i.GuestEmail,
                    Url = i.GuestUrl,
                }).SingleOrDefault();
            if (data is null)
            {
                return OperationResult.Fail<GuestUser>("not found");
            }
            return OperationResult.Ok(data);
        }

        public ScoreSubtotal Count(int itemId, byte itemType = 0)
        {
            var data = db.Comments.Where(i => i.ItemId == itemId && i.ItemType == itemType)
                .GroupBy(i => i.Score)
                .Select(i => new ScoreCount()
                {
                    Score = i.Key,
                    Count = i.Count()
                }).ToArray();
            var args = new ScoreSubtotal();
            var total = 0;
            foreach (var item in data)
            {
                total += item.Count * item.Score;
                args.Total += item.Count;
                if (item.Score > 7)
                {
                    args.Good += item.Count;
                    continue;
                }
                if (item.Score < 3)
                {
                    args.Bad += item.Count;
                    continue;
                }
                args.Middle += item.Count;
            }
            args.Avg = args.Total > 0 ? total / args.Total : 10;
            args.FavorableRate = args.Total > 0 ?
                (float)Math.Ceiling((double)(args.Good * 100 / args.Total)) : 100;
            return args;
        }

    }
}
