using Microsoft.EntityFrameworkCore;
using NetDream.Modules.MicroBlog.Entities;
using NetDream.Modules.MicroBlog.Forms;
using NetDream.Modules.MicroBlog.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace NetDream.Modules.MicroBlog.Repositories
{
    public class MicroRepository(MicroBlogContext db, 
        IClientContext client, 
        IUserRepository userStore,
        IZoneRepository zoneStore,
        ILinkRuler ruler,
        ISystemBulletin bulletin,
        IGlobeOption option)
    {
        public const byte LOG_TYPE_MICRO_BLOG = 0;
        public const byte LOG_TYPE_COMMENT = 1;

        public const byte LOG_ACTION_RECOMMEND = 1;
        public const byte LOG_ACTION_COLLECT = 2;
        public const byte LOG_ACTION_AGREE = 3;
        public const byte LOG_ACTION_DISAGREE = 4;
        public CommentProvider Comment()
        {
            return new CommentProvider(db, client, userStore);
        }

        public ActionLogProvider Log()
        {
            return new ActionLogProvider(db, client);
        }

        private bool CheckZone(int blog)
        {
            return CheckZone(db.Blogs.Where(i => i.Id == blog).Single());
        }

        private bool CheckZone(BlogEntity blog)
        {
            if (blog.UserId == client.UserId)
            {
                return true;
            }
            return zoneStore.IsZone(client.UserId, blog.ZoneId);
        }

        public IPage<PostListItem> GetSelfList(QueryForm form)
        {
            return GetList(new PostQueryForm()
            {
                Keywords = form.Keywords,
                PerPage = form.PerPage,
                Page = form.Page,
                User = client.UserId,
            });
        }

        public IPage<PostListItem> GetList(PostQueryForm form)
        {
            if (client.UserId == 0)
            {
                return new Page<PostListItem>();
            }
            var zoneId = zoneStore.GetZone(client.UserId);
            if (zoneId == 0)
            {
                if (form.User != client.UserId)
                {
                    return new Page<PostListItem>();
                }
                form.User = client.UserId;
            }
            var query = db.Blogs.When(form.Id > 0, i => i.Id == form.Id)
                .Where(i => i.ZoneId == zoneId || i.UserId == client.UserId);
            if (form.Id == 0)
            {
                query = query.Search(form.Keywords, "content")
                .When(form.User > 0, i => i.UserId == form.User);
                if (form.Topic > 0)
                {
                    var ids = db.BlogTopics.Where(i => i.TopicId == form.Topic)
                        .Pluck(i => i.MicroId);
                    if (ids.Length == 0)
                    {
                        return new Page<PostListItem>(0, form);
                    }
                    query = query.Where(i => ids.Contains(i.Id));
                }
            }
            query = form.Sort switch
            {
                "recommend" or "best" => query.OrderByDescending(i => i.RecommendCount),
                "hot" => query.OrderByDescending(i => i.CommentCount),
                _ => query.OrderByDescending(i => i.CreatedAt)
            };
            var items = query.ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            IncludeAttachment(items.Items);
            return items;
        }

        public IOperationResult<BlogEntity> ManageChange(int id, byte status)
        {
            var model = db.Blogs.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<BlogEntity>.Fail("id is error");
            }
            model.Status = status;
            db.Update(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<PostListItem> Get(int id)
        {
            if (client.UserId == 0)
            {
                return OperationResult<PostListItem>.Fail("id 错误");
            }
            var model = db.Blogs.Where(i => i.Id == id).SingleOrDefault();
            if (model is null || !CheckZone(model))
            {
                return OperationResult<PostListItem>.Fail("id 错误");
            }
            var res = new PostListItem(model);
            userStore.Include(res);
            res.Attachments = db.Attachments.Where(i => i.MicroId == model.Id).ToArray();
            return OperationResult.Ok(res);
        }

        private void IncludeAttachment(PostListItem[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var idItems = items.Select(i => i.Id).ToArray();
            var data = db.Attachments.Where(i => idItems.Contains(i.MicroId)).ToArray();
            if (data.Length == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                item.Attachments = data.Where(i => i.MicroId == item.Id).ToArray();
            }
        }

        /// <summary>
        /// 不允许频繁发布
        /// </summary>
        /// <returns></returns>
        public bool CanPublish()
        {
            if (!option.TryGet<int>("micro_time_limit", out var limit))
            {
                limit = 300;
            }
            if (limit < 10)
            {
                return true;
            }
            var time = db.Blogs.Where(i => i.UserId == client.UserId)
                .Max(i => i.CreatedAt);
            return time == 0 || time < client.Now - limit;
        }

        public IOperationResult<BlogEntity> Create(PublishForm data)
        {
            return CreateWithRule(data.Content, [], data.File);
        }
        public IOperationResult<BlogEntity> Create(string content, 
            string[] files, string source = "web")
        {
            return CreateWithRule(content, [], files, source);
        }

        public IOperationResult<BlogEntity> CreateWithRule(string content, 
            LinkExtraRule[] extraRules, string[] files, string source = "web")
        {
            var model = new BlogEntity()
            {
                UserId = client.UserId,
                ZoneId = zoneStore.GetZone(client.UserId),
                Content = HttpUtility.HtmlEncode(content),
                Source = source,
                CreatedAt = client.Now,
                UpdatedAt = client.Now
            };
            db.Blogs.Add(model);
            db.SaveChanges();
            extraRules = [
                ..extraRules,
                ..At(content, model.Id),
                ..Topic(content, model.Id),
                ..ruler.FormatEmoji(content)
            ];
            if (extraRules.Length == 0)
            {
                model.ExtraRule = JsonSerializer.Serialize(extraRules);
                db.Blogs.Update(model);
                db.SaveChanges();
            }
            if (files.Length == 0)
            {
                return OperationResult.Ok(model);
            }
            foreach (var file in files)
            {
                var thumb = file;
                //if (is_array(file))
                //{
                //    thumb = file["thumb"];
                //    file = file["file"];
                //}
                if (string.IsNullOrWhiteSpace(file))
                {
                    continue;
                }
                db.Attachments.Add(new AttachmentEntity()
                {
                    Thumb = thumb,
                    File = file,
                    MicroId = model.Id
                });
            }
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOperationResult<CommentListItem> CommentSave(CommentForm data)
        {
            var model = db.Blogs.Where(i => i.Id == data.MicroId).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<CommentListItem>.Fail("id 错误");
            }
            var content = HttpUtility.HtmlEncode(data.Content);
            LinkExtraRule[] extraRules = [
                .. At(content, 0, 1),
                ..ruler.FormatEmoji(content)
            ];
            var res = Comment().Save(new Shared.Providers.Forms.CommentEditForm()
            {
                Content = content,
                ParentId = data.ParentId,
                TargetId = model.Id,
                ExtraRule = extraRules
            });
            if (!res.Succeeded)
            {
                return OperationResult<CommentListItem>.Fail("id 错误");
            }
            var comment = res.Result;
            if (data.IsForward)
            {
                if (model.ForwardId > 0)
                {
                    var sourceUser = userStore.Get(model.UserId);
                    content = string.Format("{0}// @{1} : {2}", content, sourceUser.Name, model.Content);
                }
                var forwardModel = new BlogEntity()
                {
                    UserId = client.UserId,
                    Content = content,
                    ForwardId = model.ForwardId > 0 ? model.ForwardId :
                        model.Id,
                    ForwardCount = 1,
                    Source = "web"
                };
                db.Blogs.Save(forwardModel, client.Now);
                db.SaveChanges();
                extraRules = [
                    .. At(content, 0),
                    .. Topic(content, forwardModel.Id),
                    ..ruler.FormatEmoji(content)
                ];
                if (extraRules.Length > 0)
                {
                    forwardModel.ExtraRule = JsonSerializer.Serialize(extraRules);
                    db.Blogs.Save(forwardModel, client.Now);
                    db.SaveChanges();
                }
                model.ForwardCount++;
            }
            At(content, model.Id);
            model.CommentCount++;
            db.Blogs.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(comment);
        }

        public IOperationResult<PostListItem> Collect(int id)
        {
            var model = db.Blogs.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<PostListItem>.Fail("id 错误");
            }
            if (model.UserId == client.UserId)
            {
                return OperationResult<PostListItem>.Fail("自己无法收藏");
            }
            if (!CheckZone(model))
            {
                return OperationResult<PostListItem>.Fail("数据错误");
            }
            var res = new PostListItem(model);
            var status = Log().ToggleLog(LOG_TYPE_MICRO_BLOG, LOG_ACTION_COLLECT, id);
            if (status > 0)
            {
                model.CollectCount++;
                res.IsCollected = true;
            }
            else
            {
                model.CollectCount--;
                res.IsCollected = false;
            }
            db.Blogs.Save(model);
            res.CollectCount = model.CollectCount;
            return OperationResult.Ok(res);
        }

        public IOperationResult RemoveSelf(int id)
        {
            var model = db.Blogs.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("id 错误");
            }
            db.Blogs.Remove(model);
            Comment().RemoveByTarget(id);
            db.BlogTopics.Where(i => i.MicroId == id).ExecuteDelete();
            db.Attachments.Where(i => i.MicroId == id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Blogs.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("id 错误");
            }
            db.Blogs.Remove(model);
            Comment().RemoveByTarget(id);
            db.BlogTopics.Where(i => i.MicroId == id).ExecuteDelete();
            db.Attachments.Where(i => i.MicroId == id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult DeleteComment(int id)
        {
            var comment = Comment().Get(id);
            if (comment is null)
            {
                return OperationResult.Fail("id 错误");
            }
            var model = db.Blogs.Where(i => i.Id == comment.TargetId).SingleOrDefault();
            if (model?.UserId != client.UserId && comment.UserId != client.UserId)
            {
                return OperationResult.Fail("无法删除");
            }
            Comment().Remove(comment.Id);
            if (model is not null)
            {
                model.CommentCount--;
                db.Blogs.Update(model);
                db.SaveChanges();
            }
            return OperationResult.Ok();
        }


        public IOperationResult<BlogEntity> Recommend(int id)
        {
            var model = db.Blogs.Where(i => i.Id == id).SingleOrDefault();
            if (model is null || !CheckZone(model))
            {
                return OperationResult.Fail<BlogEntity>("id 错误");
            }
            var res = Log().ToggleLog(LOG_TYPE_MICRO_BLOG, LOG_ACTION_RECOMMEND, id);
            model.RecommendCount += res > 0 ? 1 : -1;
            db.Blogs.Update(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /// <summary>
        /// 转发
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IOperationResult<BlogEntity> Forward(ForwardForm data)
        {
            var source = db.Blogs.Where(i => i.Id == data.Id).SingleOrDefault();
            if (source is null || !CheckZone(source))
            {
                return OperationResult<BlogEntity>.Fail("id 错误");
            }
            var model = new BlogEntity()
            {
                ZoneId = zoneStore.GetZone(client.UserId),
                UserId = client.UserId,
                Content = HttpUtility.HtmlEncode(data.Content),
                ForwardId = source.Id,
                ForwardCount = 1,
                Source = "web"
            };
            db.Blogs.Save(model, client.Now);
            db.SaveChanges();
            if (model.Id == 0)
            {
                return OperationResult<BlogEntity>.Fail("转发失败");
            }
            LinkExtraRule[] extraRules = [
                .. At(data.Content, model.Id),
                ..Topic(data.Content, model.Id),
                ..ruler.FormatEmoji(data.Content)
            ];
            if (extraRules.Length > 0)
            {
                model.ExtraRule = JsonSerializer.Serialize(extraRules);
                db.Blogs.Save(model, client.Now);
                db.SaveChanges();
            }
            if (data.IsComment)
            {
                Comment().Save(new Shared.Providers.Forms.CommentEditForm()
                {
                    Content = model.Content,
                    ExtraRule = extraRules,
                    TargetId = source.Id,
                });
                source.CommentCount++;
            }
            source.ForwardCount++;
            db.Blogs.Save(source, client.Now);
            db.SaveChanges();
            At(data.Content, model.Id);
            return OperationResult.Ok(model);
        }


        /// <summary>
        /// at 人
        /// </summary>
        /// <param name="content"></param>
        /// <param name="itemId"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public LinkExtraRule[] At(string content, int itemId = 0, int itemType = 0)
        {
            if (string.IsNullOrWhiteSpace(content) || !content.Contains('@'))
            {
                return [];
            }
            var matches = Regex.Matches(content, @"@(\S+?)\s");
            if (matches.Count == 0)
            {
                return [];
            }
            var names = matches.Select(i => new KeyValuePair<string, string>(i.Groups[1].Value, i.Value)).ToDictionary();
            var users = userStore.Get(names.Keys.ToArray());
            if (users.Length == 0)
            {
                return [];
            }
            var rules = new List<LinkExtraRule>();
            var currentUser = client.UserId;
            var userIds = new List<int>();
            foreach (var item in users)
            {
                if (item.Id != currentUser)
                {
                    userIds.Add(item.Id);
                }
                rules.Add(
                    ruler.FormatUser(names[item.Name], item.Id)
                );
            }
            if (itemId < 1 || userIds.Count == 0)
            {
                return rules.ToArray();
            }
            if (itemType < 1)
            {
                bulletin.SendAt(userIds.ToArray(), "我在微博提到了你", "micro/" + itemId);
            }
            return rules.ToArray();
        }

        /// <summary>
        /// 生成话题规则
        /// </summary>
        /// <param name="content"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public LinkExtraRule[] Topic(string content, int id)
        {
            if (string.IsNullOrWhiteSpace(content) || !content.Contains('#'))
            {
                return [];
            }
            var matches = Regex.Matches(content, @"#(\S+?)#(\s|$)");
            if (matches.Count == 0)
            {
                return [];
            }
            var items = new Dictionary<string, List<string>>();
            foreach (Match match in matches)
            {
                var name = match.Groups[1].Value.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                if (!items.TryGetValue(name, out var args))
                {
                    args = new List<string>();
                    items.Add(name, args);
                }
                args.Add(match.Value);
            }
            var store = new TopicRepository(db, client);
            var topicItems = store.Save(items.Keys.ToArray());
            store.BindTag(id, topicItems);
            if (topicItems.Length == 0)
            {
                return [];
            }
            var topicMaps = topicItems.ToDictionary(i => i.Name, i => i.Id);
            var rules = new List<LinkExtraRule>();
            foreach (var item in items)
            {
                if (!topicMaps.TryGetValue(item.Key, out var topicId))
                {
                    continue;
                }
                foreach (var it in item.Value)
                {
                    rules.Add(FormatTopic(it, topicId));
                }
            }
            return [..rules];
        }

        /// <summary>
        /// 创建分享
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOperationResult<BlogEntity> Share(ShareForm data)
        {
            var tag = HttpUtility.HtmlEncode(data.Title);
            var content = string.Format("{0} \n【{1}】{2}{3}",
                HttpUtility.HtmlEncode(data.Content), tag,
                string.IsNullOrWhiteSpace(data.Sharesource) ? string.Empty : 
                string.Format(" - 分享自 @{0} ", HttpUtility.HtmlEncode(data.Sharesource.Trim()))
                , HttpUtility.HtmlEncode(data.Summary));
            LinkExtraRule[] extraRule = [
                ruler.FormatLink(tag, data.Url)
            ];
            return CreateWithRule(content, extraRule, data.Pics);
        }

        public string RenderUser(LinkExtraRule rule)
        {
            return string.Format("<a href=\"/micro?user={0}\">{1}</a>", rule.User, rule.Word);
        }

        public string RenderExtra(LinkExtraRule rule)
        {
            if (rule.TryGet(TopicRepository.LINK_RULE_KEY, out var val))
            {
                return string.Format("<a href=\"/micro?topic={0}\">{1}</a>", val, rule.Word);
            }
            return string.Empty;
        }

        public LinkExtraRule FormatTopic(string word, int topic)
        {
            return new LinkExtraRule(word)
            {
                ExtraKey = TopicRepository.LINK_RULE_KEY,
                ExtraValue = topic.ToString()
            };
        }

        public string[] Suggest(string keywords)
        {
            return db.Topics.Search(keywords, "name").Take(5).Select(i => i.Name).ToArray();
        }
    }
}
