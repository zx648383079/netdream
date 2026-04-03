using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Comment.Entities;
using NetDream.Modules.Comment.Forms;
using NetDream.Modules.Comment.Models;
using NetDream.Shared.Events;
using NetDream.Shared.Events.Notifications;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
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
        IEventBus mediator): ICommentRepository
    {

        private void IncludeReply(IEnumerable<CommentListItem> items)
        {
            var idItems = items.Select(item => item.Id);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Comments.Where(i => idItems.Contains(i.ParentId)).ToArray()
                .Select(i => new CommentListItem(i)).ToArray();
            if (data.Length == 0)
            {
                return;
            }
            userStore.Include(data);
            foreach (var item in items)
            {
                item.Replies = [..data.Where(i => i.ParentId == item.Id)];
            }
        }

        public IOperationResult<CommentEntity> Create(CommentForm data)
        {
            var model = new CommentEntity()
            {
                ItemId = data.TargetId,
                ItemType = data.TargetType,
                Content = data.Content,
            };
            if (client.UserId > 0)
            {
                model.UserId = client.UserId;
                model.GuestName = userStore.Get(client.UserId).Name;
            }
            if (data.Parent > 0)
            {
                var parent = db.Comments.Where(i => i.Id == data.Parent).SingleOrDefault();
                if (parent?.ParentId > 0)
                {
                    parent = db.Comments.Where(i => i.Id == parent.ParentId).SingleOrDefault();
                }
                model.ParentId = parent?.Id ?? 0;
            }
            var last = db.Comments.Where(i => i.ItemId == data.TargetId && i.ItemType == data.TargetType && i.ParentId == model.ParentId)
                .OrderByDescending(i => i.Position).SingleOrDefault();
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


        /// <summary>
        /// 用于后台管理
        /// </summary>
        /// <param name="blog"></param>
        /// <param name="keywords"></param>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IPage<CommentListItem> AdvancedList(CommentQueryForm form)
        {
            var items = db.Comments.When(form.TargetId > 0, i => i.ItemId == form.TargetId && i.ItemType == form.TargetType)
                .Search(form.Keywords, "content")
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => new CommentListItem(i));
            userStore.Include(items.Items);
            return items;
        }

        public void AdvancedRemove(int id)
        {
            db.Comments.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }


        public IOperationResult<CommentEntity> AdvancedToggle(int id)
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



        public int Count(ModuleTargetType type)
        {
            return db.Comments.Where(i => i.ItemType == (byte)type).Count();
        }

        public int Count(ModuleTargetType type, DateTime startAt)
        {
            var start = TimeHelper.TimestampFrom(startAt);
            return db.Comments.Where(i => i.ItemType == (byte)type && i.CreatedAt >= start).Count();
        }

        public int Count(ModuleTargetType type, DateTime startAt, DateTime endAt)
        {
            var start = TimeHelper.TimestampFrom(startAt);
            var end = TimeHelper.TimestampFrom(endAt);
            return db.Comments.Where(i => i.ItemType == (byte)type && i.CreatedAt >= start && i.CreatedAt <= end).Count();
        }

        public IPage<ICommentItem> Search(ModuleTargetType type, int article, IQueryForm form)
        {
            var (sort, order) = CheckSortOrder(form);
            var parent = 0;
            var user = 0;
            if (form is CommentQueryForm q)
            {
                parent = q.Parent;
                user = q.User;
                if (q.TargetId > 0)
                {
                    article = q.TargetId;
                    type = (ModuleTargetType)q.TargetType;
                }
            }
            var query = db.Comments.Where(i => i.Status == (byte)ReviewStatus.Approved
            && i.ItemType == (byte)type && i.ItemId == article)
                .When(parent >= 0, i => i.ParentId == parent)
                .When(user > 0, i => i.UserId == user);
            query = query.OrderBy<CommentEntity, int>(sort, order);
            var data = query.ToPage(form, i => new CommentListItem(i));
            userStore.Include(data.Items);
            if (parent == 0)
            {
                IncludeReply(data.Items);
            }
            return data.Cast<ICommentItem>();
        }

        public ICommentItem[] Get(ModuleTargetType type, int article, IQueryForm form)
        {
            var (sort, order) = CheckSortOrder(form);
            var parent = 0;
            var user = 0;
            if (form is CommentQueryForm q)
            {
                parent = q.Parent;
                user = q.User;
                if (q.TargetId > 0)
                {
                    article = q.TargetId;
                    type = (ModuleTargetType)q.TargetType;
                }
            }
            var query = db.Comments.Where(i => i.Status == (byte)ReviewStatus.Approved
            && i.ItemType == (byte)type)
                .When(article > 0, i => i.ItemId == article)
                .When(parent >= 0, i => i.ParentId == parent)
                .When(user > 0, i => i.UserId == user);
            query = query.OrderBy<CommentEntity, int>(sort, order);
            var data = query.Take(form.PerPage).ToArray();
            var items = data.Select(i => new CommentListItem(i)).ToArray();
            userStore.Include(items);
            return items;
        }

        public ICommentItem[] Get(ModuleTargetType type, IQueryForm form)
        {
            return Get(type, 0, form);
        }

        public IOperationResult Create(int user, ModuleTargetType type, int article, string content, int parent = 0)
        {
            return Create(user, type, article, new CommentForm()
            {
                Content = content,
                Parent = parent,
            });
        }

        public IOperationResult Create(int user, ModuleTargetType type, int article, string content, LinkExtraRule[] rules, int parent = 0)
        {
            var model = new CommentEntity()
            {
                ItemId = article,
                ItemType = (byte)type,
                Content = content,
            };
            if (client.UserId > 0)
            {
                model.UserId = client.UserId;
                model.GuestName = userStore.Get(client.UserId).Name;
            }
            if (parent > 0)
            {
                var next = db.Comments.Where(i => i.Id == parent).SingleOrDefault();
                if (next?.ParentId > 0)
                {
                    next = db.Comments.Where(i => i.Id == next.ParentId).SingleOrDefault();
                }
                model.ParentId = next?.Id ?? 0;
            }
            var last = db.Comments.Where(i => i.ItemId == model.ItemId && i.ItemType == model.ItemType && i.ParentId == model.ParentId)
                .OrderByDescending(i => i.Position).SingleOrDefault();
            model.Position = last is null ? 1 : (last.Position + 1);
            model.Status = (byte)ReviewStatus.None;
            model.ExtraRule = JsonSerializer.Serialize(rules);
            db.Comments.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult Create(int user, ModuleTargetType type, int article, ICommentForm form)
        {
            var model = new CommentEntity()
            {
                ItemId = article,
                ItemType = (byte)type,
                Content = form.Content,
            };
            if (client.UserId > 0)
            {
                model.UserId = client.UserId;
                model.GuestName = userStore.Get(client.UserId).Name;
            }
            if (form is CommentForm f && f.Parent > 0)
            {
                var parent = db.Comments.Where(i => i.Id == f.Parent).SingleOrDefault();
                if (parent?.ParentId > 0)
                {
                    parent = db.Comments.Where(i => i.Id == parent.ParentId).SingleOrDefault();
                }
                model.ParentId = parent?.Id ?? 0;
            }
            var last = db.Comments.Where(i => i.ItemId == model.ItemId && i.ItemType == model.ItemType && i.ParentId == model.ParentId)
                .OrderByDescending(i => i.Position).SingleOrDefault();
            model.Position = last is null ? 1 : (last.Position + 1);
            model.Status = (byte)ReviewStatus.None;
            db.Comments.Save(model);
            db.SaveChanges();
            LinkExtraRule[] extraRules = [..
                At(model.Content, model.ItemId, model.ParentId),
                ..ruler.FormatEmoji(model.Content)
            ];
            if (extraRules.Length != 0)
            {
                model.ExtraRule = JsonSerializer.Serialize(extraRules);
                db.Comments.Save(model);
                db.SaveChanges();
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult Reply(int user, ModuleTargetType type, int article, int parent, string content)
        {
            return Create(user, type, article, new CommentForm()
            {
                Content = content,
                Parent = parent,
            });
        }

        public IOperationResult Remove(int user, ModuleTargetType type, int comment)
        {
            
            var model = db.Comments.Where(i => i.Id == comment).When(user > 0, i => i.UserId == user).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("评论删除失败");
            }
            db.Comments.Remove(model);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult<IUser> LastCommentator(string email)
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
                return OperationResult.Fail<IUser>("not found");
            }
            return OperationResult.Ok<IUser>(data);
        }

        public IOperationResult<AgreeResult> Toggle(int user, ModuleTargetType type, 
            int comment, bool agree)
        {
            var model = db.Comments.Where(i => i.Id == comment).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<AgreeResult>.Fail("评论不存在");
            }
            var res = interact.Update(user, ModuleTargetType.Comment, comment,
                agree ? InteractType.Agree : InteractType.Disagree,
                [InteractType.Disagree, InteractType.Agree]);
            var data = new AgreeResult()
            {
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            if (res == RecordToggleType.Deleted)
            {
                data.AgreeType = AgreeType.None;
            } else
            {
                data.AgreeType = agree ? AgreeType.Agree : AgreeType.Disagree;
            }
            switch (data.AgreeType)
            {
                case AgreeType.Agree:
                    data.AgreeCount++;
                    if (res == RecordToggleType.Updated)
                    {
                        data.DisagreeCount--;
                    }
                    break;
                case AgreeType.Disagree:
                    data.DisagreeCount++;
                    if (res == RecordToggleType.Updated)
                    {
                        data.AgreeCount--;
                    }
                    break;
                default:
                    break;

            }
            db.Comments.Where(i => i.Id == model.Id)
                  .ExecuteUpdate(setters => setters
                  .SetProperty(i => i.AgreeCount, data.AgreeCount)
                  .SetProperty(i => i.DisagreeCount, data.DisagreeCount));
            return OperationResult.Ok(data);
        }

        public IOperationResult Report(int user, ModuleTargetType type, int comment)
        {
            var model = db.Comments.Where(i => i.Id == comment).Single();
            mediator.Publish(ReportRequest.Create(client, ModuleTargetType.Comment, comment,
                string.Format("“{0}”", model.Content), "举报博客评论"));
            return OperationResult.Ok();
        }

        public IScoreSubtotal Score(ModuleTargetType type, int article)
        {
            var data = db.Comments.Where(i => i.ItemId == article && i.ItemType == (byte)type && i.ParentId == 0 && i.Score > 0)
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

        private static (string, string) CheckSortOrder(IQueryForm form)
        {
            return form.Sort switch
            {
                "new" => ("created_at", "desc"),
                "recommend" or "best" or "hot" => ("agree_count", "desc"),
                _ => SearchHelper.CheckSortOrder(form, ["created_at", "id", "agree_count"]),
            };
        }

        public int Count(int user, ModuleTargetType type)
        {
            return db.Comments.Where(i => i.UserId == user && i.ItemType == (byte)type).Count();
        }

        public IOperationResult<ICommentSource> GetSource(ModuleTargetType type, int comment)
        {
            throw new NotImplementedException();
        }

        public IOperationResult RemoveArticle(int user, ModuleTargetType type, int article)
        {
            db.Comments.Where(i => i.ItemType == (byte)type && i.ItemId == article).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }
    }
}
