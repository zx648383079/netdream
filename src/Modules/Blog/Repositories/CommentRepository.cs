using Mysqlx.Crud;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Forms;
using NetDream.Modules.Blog.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Models;
using NetDream.Shared.Repositories;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetDream.Modules.Blog.Repositories
{
    public class CommentRepository(IDatabase db, 
        IUserRepository userStore,
        IClientContext environment,
        IGlobeOption option,
        ILinkRuler ruler,
        ISystemBulletin bulletin,
        ISystemFeedback feedback,
        LogRepository logStore)
    {
        public Page<CommentModel> GetList(int blogId, int parentId = 0, 
            bool isHot = false, 
            string sort = MigrationTable.COLUMN_CREATED_AT, 
            string order = "desc", int page = 1, int perPage = 20)
        {
            (sort, order) = SearchHelper.CheckSortOrder(sort, order, 
                [MigrationTable.COLUMN_CREATED_AT, "id", "agree_count"]);

            var sql = new Sql();
            sql.Select("*")
                .From<CommentEntity>(db)
                .Where("approved=1")
                .Where("blog_id=@0 AND parent_id=@1", blogId, parentId);
            if (isHot)
            {
                sql.Where("agree_count>0").OrderBy("agree_count DESC");
            }
            sql.OrderBy($"{sort} {order}");
            var items = db.Page<CommentModel>(page, perPage, sql);
            userStore.WithUser(items.Items);
            if (parentId == 0)
            {
                WithReply(items.Items);
            }
            return items;
        }

        private void WithReply(IEnumerable<CommentModel> items)
        {
            var idItems = items.Select(item => item.Id);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Fetch<CommentModel>($"WHERE parent_id IN({string.Join(',', idItems)})");
            if (!data.Any())
            {
                return;
            }
            userStore.WithUser(data);
            foreach (var item in items)
            {
                item.Replies = [..data.Where(i => i.ParentId == item.Id)];
            }
        }

        private void WithBlog(IEnumerable<CommentModel> items)
        {
            var idItems = items.Select(item => item.BlogId);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Fetch<BlogEntity>($"WHERE id IN({string.Join(',', idItems)})");
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

        public CommentEntity Create(CommentForm data)
        {
            if (!CanComment(data.BlogId))
            {
                throw new Exception("不允许评论！");
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
            if (environment.UserId > 0)
            {
                model.UserId = environment.UserId;
                model.Name = userStore.Get(environment.UserId).Name;
            }
            if (data.ParentId > 0)
            {
                var parent = db.SingleById<CommentEntity>(data.ParentId);
                if (parent.ParentId > 0)
                {
                    parent = db.SingleById<CommentEntity>(parent.ParentId);
                }
                model.ParentId = parent.Id;
            }
            var last = db.Single<CommentEntity>(
                "WHERE blog_id=@0 and parent_id=@1 ORDER BY position desc", data.BlogId, model.ParentId);
            model.Position = last is null ? 1 : (last.Position + 1);
            model.Approved = option.Get<bool>("comment_approved") ? 0 : 1;
            db.Save(model);
            db.UpdateWhere<BlogEntity>("comment_count=comment_count+1", "id=@0", data.BlogId);
            LinkExtraRule[] extraRules = [..
                At(data.Content, model.BlogId, model.ParentId),
                ..ruler.FormatEmoji(data.Content)
            ];
            if (extraRules.Any())
            {
                // model.ExtraRule = extraRules;
                db.UpdateById<CommentEntity>(model.Id, new Dictionary<string, object>()
                {
                    { "extra_rules", JsonSerializer.Serialize(extraRules)}
                });
            }
            return model;
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
            var commentPosition = new Dictionary<string, string>();
            foreach (Match match in matches)
            {
                if (Regex.IsMatch(match.Groups[1].Value, @"^\d+#"))
                {
                    commentPosition[match.Groups[1].Value[0..-1]] = match.Value;
                    continue;
                }
                names[match.Groups[1].Value] = match.Value;
            }
            return [..AtUser(names, blogId), ..AtPosition(commentPosition, commentId)];
        }

        protected IEnumerable<LinkExtraRule> AtPosition(Dictionary<string, string> items, 
            int commentId)
        {
            if (commentId < 1)
            {
                return [];
            }
            var sql = new Sql();
            sql.Select("id", "position")
                .From<CommentEntity>(db)
                .Where("parent_id=@0", commentId)
                .WhereIn("postion", items.Keys.ToArray());
            var commentItems = db.Fetch<CommentEntity>(sql);
            if (commentItems is null || commentItems.Count == 0)
            {
                return [];
            }
            return commentItems.Select(item => {
                return ruler.FormatId(items[item.Position.ToString()], "comment-" + item.Id);
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
            var currentUser = environment.UserId;
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

        public IList<CommentModel> GetHot(int blogId, int limit = 4)
        {
            var sql = new Sql();
            sql.Select("*").From<CommentEntity>(db)
                .Where("blog_id=@0 and parent_id=0 and agree_count>0")
                .OrderBy("agree_count desc")
                .Limit(limit);
            var items = db.Fetch<CommentModel>(sql);
            userStore.WithUser(items);
            return items;
        }

        /**
         * 用于后台管理
         * @param int blog
         * @param string keywords
         * @param string email
         * @param string name
         */
        public Page<CommentModel> CommentList(int blog = 0, 
            string keywords = "", 
            string email = "", 
            string name = "",
            int page = 1)
        {
            var sql = new Sql();
            sql.Select("*").From<CommentEntity>(db);
            if (blog > 0)
            {
                sql.Where("blog_id=@0", blog);
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                sql.Where("email=@0", email);
            }
            SearchHelper.Where(sql, "content", keywords);
            SearchHelper.Where(sql, "name", name);
            sql.OrderBy("id DESC");
            var items = db.Page<CommentModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            WithBlog(items.Items);
            return items;
        }

        public void Remove(int id)
        {
            db.Delete<CommentEntity>(id);
        }

        /**
         * 前台删除，博主或发表人
         * @param int id
         */
        public void RemoveSelf(int id)
        {
            var model = db.SingleById<CommentEntity>(id);
            if (model is null)
            {
                throw new Exception("评论删除失败");
            }
            if (model.UserId > 0 && model.UserId == environment.UserId)
            {
                db.Delete<CommentEntity>(id);
                return;
            }
            if (!IsSelfBlog(model.BlogId))
            {
                throw new Exception("评论删除失败");
            }
            db.Delete<CommentEntity>(id);
        }

        public bool IsSelfBlog(int blogId)
        {
            return db.FindCount<BlogEntity>("id=@0 and user_id=@1", blogId, environment.UserId) > 0;
        }

        public IList<CommentModel> NewList()
        {
            var sql = new Sql();
            sql.Select("*").From<CommentEntity>(db)
                .Where("approved=1")
                .OrderBy("created_at desc")
                .Limit(4);
            var items = db.Fetch<CommentModel>(sql);
            WithBlog(items);
            return items;
        }

        public void Report(int id)
        {
            var model = db.SingleById<CommentEntity>(id);
            feedback.Report(ModuleModelType.TYPE_BLOG_COMMENT, id,
                string.Format("“{0}”", model.Content), "举报博客评论");
        }

        public CommentLog Disagree(int id)
        {
            var model = db.SingleById<CommentEntity>(id);
            if (model is null)
            {
                throw new Exception("评论不存在");
            }
            var res = logStore.ToggleLog(environment.UserId, id, LogRepository.TYPE_COMMENT,
                LogRepository.ACTION_DISAGREE, 
                [LogRepository.ACTION_AGREE, LogRepository.ACTION_DISAGREE]);
            if (res < 1)
            {
                model.DisagreeCount--;
            }
            else if (res == 1)
            {
                model.AgreeCount--;
                model.DisagreeCount++;
            }
            else if (res == 2)
            {
                model.DisagreeCount++;
            }
            db.UpdateById<CommentEntity>(id, new Dictionary<string, object>()
            {
                { "agree_count", model.AgreeCount},
                { "disagree_count", model.DisagreeCount},
            });
            var log = new CommentLog()
            {
                Id = model.Id,
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            if (res > 0)
            {
                log.AgreeType = CommentProvider.LOG_ACTION_DISAGREE;
            }
            return log;
        }

        public CommentLog Agree(int id)
        {
            var model = db.SingleById<CommentEntity>(id);
            if (model is null)
            {
                throw new Exception("评论不存在");
            }
            var res = logStore.ToggleLog(environment.UserId, id, LogRepository.TYPE_COMMENT,
                LogRepository.ACTION_AGREE,
                [LogRepository.ACTION_AGREE, LogRepository.ACTION_DISAGREE]);
            if (res < 1)
            {
                model.AgreeCount--;
            }
            else if (res == 1)
            {
                model.AgreeCount++;
                model.DisagreeCount--;
            }
            else if (res == 2)
            {
                model.AgreeCount++;
            }
            db.UpdateById<CommentEntity>(id, new Dictionary<string, object>()
            {
                { "agree_count", model.AgreeCount},
                { "disagree_count", model.DisagreeCount},
            });
            var log = new CommentLog()
            {
                Id = model.Id,
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            if (res > 0)
            {
                log.AgreeType = CommentProvider.LOG_ACTION_AGREE;
            }
            return log;
        }

        public CommentEntity? ManageToggle(int id)
        {
            var model = db.SingleById<CommentEntity>(id);
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
            if (val == 2 && environment.UserId == 0)
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
            var val = db.FindScalar<int, BlogMetaEntity>("content", "blog_id=@0 AND name=@1",
                blogId, "comment_status");
            return CommentStatus(val);
        }

        /**
         * 获取用户的最好使用信息
         * @param string email
         * @return array
         * @throws Exception
         */
        public CommentEntity LastCommentator(string email)
        {
            var sql = new Sql();
            sql.Select("name", "email", "url")
                .From<CommentEntity>(db)
                .Where("email=@0", email)
                .OrderBy("created_at DESC");
            var data = db.Single<CommentEntity>(sql);
            if (data is null)
            {
                throw new Exception("not found");
            }
            return data;
        }
    }
}
