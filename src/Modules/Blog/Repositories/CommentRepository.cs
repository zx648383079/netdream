using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Forms;
using NetDream.Modules.Blog.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Blog.Repositories
{
    public class CommentRepository(BlogContext db, 
        IUserRepository userStore,
        IClientContext client,
        IGlobeOption option,
        ILinkRuler ruler,
        ISystemBulletin bulletin,
        ISystemFeedback feedback,
        LogRepository logStore)
    {
        public IPage<CommentListItem> GetList(int blogId, int parentId = 0, 
            bool isHot = false, 
            string sort = "created_at", 
            string order = "desc", int page = 1)
        {
            (sort, order) = SearchHelper.CheckSortOrder(sort, order, 
                ["created_at", "id", "agree_count"]);

            var query = db.Comments.Where(i => i.Approved == 1 && i.BlogId == blogId && i.ParentId == parentId);
            if (isHot)
            {
                query = query.Where(i => i.AgreeCount > 0)
                    .OrderByDescending(i => i.AgreeCount);
            } else
            {
                query = query.OrderBy<CommentEntity, int>(sort, order);
            }
            var items = query.ToPage(page, i => i.Select<CommentEntity, CommentListItem>());
            userStore.Include(items.Items);
            if (parentId == 0)
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
            var data = db.Comments.Where(i => idItems.Contains(i.ParentId))
                .Select<CommentEntity, CommentListItem>().ToArray();
            if (!data.Any())
            {
                return;
            }
            userStore.Include(data);
            foreach (var item in items)
            {
                item.Replies = [..data.Where(i => i.ParentId == item.Id)];
            }
        }

        private void WithBlog(IEnumerable<CommentListItem> items)
        {
            var idItems = items.Select(item => item.BlogId);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Blogs.Where(i => idItems.Contains(i.Id))
                .Select(i => new ListLabelItem()
                {
                    Id = i.Id,
                    Name = i.Title,
                }).ToArray();
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.BlogId == it.Id)
                    {
                        item.Blog = it;
                        break;
                    }
                }
            }
        }

        public IOperationResult<CommentEntity> Create(CommentForm data)
        {
            if (!CanComment(data.BlogId))
            {
                return OperationResult<CommentEntity>.Fail("不允许评论！");
            }
            var model = new CommentEntity()
            {
                Name = data.Name,
                Email = data.Email,
                Url = data.Url,
                Content = data.Content,
                ExtraRule = data.ExtraRule,
                ParentId = data.ParentId,
                BlogId = data.BlogId,
            };
            if (client.UserId > 0)
            {
                model.UserId = client.UserId;
                model.Name = userStore.Get(client.UserId).Name;
            }
            if (data.ParentId > 0)
            {
                var parent = db.Comments.Where(i => i.Id == data.ParentId).Single();
                if (parent.ParentId > 0)
                {
                    parent = db.Comments.Where(i => i.Id == parent.ParentId).Single();
                }
                model.ParentId = parent.Id;
            }
            var last = db.Comments.Where(i => i.BlogId == data.BlogId && i.ParentId == model.ParentId)
                .OrderByDescending(i => i.Position).Single();
            model.Position = last is null ? 1 : (last.Position + 1);
            model.Approved = option.Get<bool>("comment_approved") ? 0 : 1;
            db.Comments.Save(model);
            db.SaveChanges();
            db.Blogs.Where(i => i.Id == model.BlogId)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.CommentCount, i => i.CommentCount + 1));
            LinkExtraRule[] extraRules = [..
                At(data.Content, model.BlogId, model.ParentId),
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

        public LinkExtraRule[] At(string content, int blogId, int commentId)
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
            return [..AtUser(names, blogId), ..AtPosition(commentPosition, commentId)];
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

        protected IEnumerable<LinkExtraRule> AtUser(Dictionary<string, string> names, int blogId)
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
            if (blogId < 1 || userIds.Count == 0)
            {
                return rules;
            }
            bulletin.SendAt([.. userIds], "我在博客评论总提到了你", "blog/" + blogId);
            return rules;
        }

        public CommentListItem[] GetHot(int blogId, int limit = 4)
        {
            var items = db.Comments.Where(i => i.BlogId == blogId && i.ParentId == 0 && i.AgreeCount > 0)
                .OrderByDescending(i => i.AgreeCount)
                .Take(limit).Select<CommentEntity, CommentListItem>().ToArray();
            userStore.Include(items);
            return items;
        }

        /**
         * 用于后台管理
         * @param int blog
         * @param string keywords
         * @param string email
         * @param string name
         */
        public IPage<CommentListItem> CommentList(int blog = 0, 
            string keywords = "", 
            string email = "", 
            string name = "",
            int page = 1)
        {
            var items = db.Comments.When(blog > 0, i => i.BlogId == blog)
                .When(email, i => i.Email == email)
                .Search(keywords, "content")
                .Search(name, "name")
                .OrderByDescending(i => i.Id).ToPage(page, i => i.Select<CommentEntity, CommentListItem>());
            userStore.Include(items.Items);
            WithBlog(items.Items);
            return items;
        }

        public void Remove(int id)
        {
            db.Comments.Where(i => i.Id == id).ExecuteDelete();
        }

        /**
         * 前台删除，博主或发表人
         * @param int id
         */
        public IOperationResult RemoveSelf(int id)
        {
            var model = db.Comments.Where(i => i.Id == id).Single();
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
            if (!IsSelfBlog(model.BlogId))
            {
                return OperationResult.Fail("评论删除失败");
            }
            db.Comments.Remove(model);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public bool IsSelfBlog(int blogId)
        {
            return db.Blogs.Where(i => i.Id == blogId && i.UserId == client.UserId).Any();
        }

        public CommentListItem[] NewList()
        {
            var items = db.Comments.Where(i => i.Approved == 1)
                .OrderByDescending(i => i.CreatedAt)
                .Take(4).Select<CommentEntity, CommentListItem>().ToArray();
            WithBlog(items);
            return items;
        }

        public void Report(int id)
        {
            var model = db.Comments.Where(i => i.Id == id).Single();
            feedback.Report(ModuleModelType.TYPE_BLOG_COMMENT, id,
                string.Format("“{0}”", model.Content), "举报博客评论");
        }

        public IOperationResult<AgreeResult> Disagree(int id)
        {
            var model = db.Comments.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult<AgreeResult>.Fail("评论不存在");
            }
            var res = logStore.ToggleLog(client.UserId, id, LogRepository.TYPE_COMMENT,
                LogRepository.ACTION_DISAGREE, 
                [LogRepository.ACTION_AGREE, LogRepository.ACTION_DISAGREE]);
            var data = new AgreeResult()
            {
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            if (res < 1)
            {
                data.DisagreeCount--;
                data.AgreeType = 0;
            }
            else if (res == 1)
            {
                data.AgreeCount--;
                data.DisagreeCount++;
                data.AgreeType = 2;
            }
            else if (res == 2)
            {
                data.DisagreeCount++;
                data.AgreeType = 2;
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
            var res = logStore.ToggleLog(client.UserId, id, LogRepository.TYPE_COMMENT,
                LogRepository.ACTION_AGREE,
                [LogRepository.ACTION_AGREE, LogRepository.ACTION_DISAGREE]);
            var data = new AgreeResult()
            {
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            if (res < 1)
            {
                data.AgreeCount--;
                data.AgreeType = 0;
            }
            else if (res == 1)
            {
                data.AgreeCount++;
                data.DisagreeCount--;
                data.AgreeType = 1;
            }
            else if (res == 2)
            {
                data.AgreeCount++;
                data.AgreeType = 1;
            }
            db.Comments.Where(i => i.Id == model.Id)
                .ExecuteUpdate(setters => setters
                .SetProperty(i => i.AgreeCount, data.AgreeCount)
                .SetProperty(i => i.DisagreeCount, data.DisagreeCount));
            return OperationResult.Ok(data);
        }

        public CommentEntity? ManageToggle(int id)
        {
            var model = db.Comments.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return null;
            }
            model.Approved = model.Approved < 1 ? 1 : 0;
            db.Update(model);
            return model;
        }

        public bool CanComment(int blogId)
        {
            var val = BlogCommentStatus(blogId);
            if (val == 2 && client.UserId == 0)
            {
                return false;
            }
            return val > 0;
        }

        public int CommentStatus(int status)
        {
            if (status < 1)
            {
                return 0;
            }
            var val = option.Get<int>("blog_comment");
            return val;
        }

        public int BlogCommentStatus(int blogId)
        {
            var val = db.Metas.Where(i => i.ItemId == blogId && i.Name == "comment_status")
                .Select(i => i.Content).Single();
            return CommentStatus(int.Parse(val));
        }

        /**
         * 获取用户的最后使用信息
         * @param string email
         * @return array
         * @throws Exception
         */
        public IOperationResult<CommentEntity> LastCommentator(string email)
        {
            var data = db.Comments.Where(i => i.Email == email)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => new CommentEntity()
                {
                    Name = i.Name,
                    Email = i.Email,
                    Url = i.Url,
                }).Single();
            if (data is null)
            {
                return OperationResult.Fail<CommentEntity>("not found");
            }
            return OperationResult.Ok(data);
        }
    }
}
