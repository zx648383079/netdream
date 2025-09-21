using Markdig;
using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Forms;
using NetDream.Modules.Blog.Markdown;
using NetDream.Modules.Blog.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Blog.Repositories
{
    public class BlogRepository(
        BlogContext db, 
        IClientContext client,
        IUserRepository userStore, 
        IDeeplink deeplink)
    {
        public const byte TYPE_BLOG = 0;
        public const byte TYPE_COMMENT = 1;

        public const byte ACTION_RECOMMEND = 0;
        public const byte ACTION_AGREE = 1;
        public const byte ACTION_DISAGREE = 2;

        public const byte ACTION_REAL_RULE = 3; // 是否能阅读
        public TagProvider Tag()
        {
            return new TagProvider(db);
        }

        public ActionLogProvider Log()
        {
            return new ActionLogProvider(db, client);
        }

        public IPage<BlogListItem> SelfList(BlogQueryForm form)
        {
            form.User = client.UserId;
            return GetList(form);
        }

        public IPage<BlogListItem> GetList(BlogQueryForm form)
        {
            SearchHelper.CheckSortOrder(form, ["id", "created_at"]);
            var items = db.Blogs.Search(form.Keywords, "Title")
                .OrderBy(form)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            CategoryRepository.Include(db, items.Items);
            return items;
        }

        public IPage<BlogListItem> ManageList(BlogQueryForm form)
        {
            SearchHelper.CheckSortOrder(form, ["id", "created_at"]);
            var items = db.Blogs.Search(form.Keywords, "Title")
                .OrderBy(form)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            CategoryRepository.Include(db, items.Items);
            return items;
        }

        public BlogListItem[] GetList(int[] items)
        {
            if (items.Length == 0)
            {
                return [];
            }
            var data = db.Blogs.Where(i => items.Contains(i.Id))
                .SelectAsLabel().ToArray();
            foreach (var item in data)
            {
                item.Url = deeplink.Encode($"blog/{item.Id}");
            }
            return data;
        }

        public BlogListItem[] GetNewBlogs(int count = 8)
        {
            return db.Blogs.OrderByDescending(i => i.CreatedAt)
                .SelectAsLabel().Take(count).ToArray();
        }

        public BlogListItem[] GetRelationBlogs(int sourceId)
        {
            var provider = Tag();
            var items = provider.GetRelationList(sourceId); ;
            if (items.Length == 0)
            {
                return [];
            }
            return db.Blogs.Where(i => items.Contains(i.Id) &&
            i.PublishStatus == PublishRepository.PUBLISH_STATUS_POSTED)
                .OrderByDescending(i => i.CreatedAt)
                .Take(5)
                .SelectAsLabel().ToArray();
        }

        public TagListItem[] GetTags()
        {
            var items = db.Tags.Select(i => new TagListItem()
            {
                Id = i.Id,
                Name = i.Name,
            }).ToArray();
            if (items.Length == 0)
            {
                return items;
            }
            var data = db.TagLinks.GroupBy(i => i.TagId)
                .Select(i => new KeyValuePair<int, int>(i.Key, i.Count()))
                .ToDictionary();
            foreach (var item in items)
            {
                if (data.TryGetValue(item.Id, out var res))
                {
                    item.TargetCount = res;
                }
            }
            return items;
        }

        public CategoryLabelItem[] Categories()
        {
            return db.Categories.SelectAsLabel().ToArray();
        }

        /// <summary>
        /// 前台获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="openKey"></param>
        /// <returns></returns>
        public IOperationResult<BlogModel> Get(int id, string openKey = "")
        {
            if (id <= 0)
            {
                return OperationResult<BlogModel>.Fail("数据错误");
            }
            var model = db.Blogs.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<BlogModel>.Fail("数据错误");
            }
            var pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseNetDreamExtensions(this) // 添加自定义代码块渲染器
                    .Build();
            return OperationResult.Ok(new BlogModel()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Content = Markdig.Markdown.ToHtml(model.Content, pipeline),
                ClickCount = model.ClickCount,
                RecommendCount = model.RecommendCount,
                CommentCount = model.CommentCount,
                CreatedAt = model.CreatedAt,
                User = userStore.Get(model.Id),
                Term = db.Categories.Where(i => i.Id == model.TermId)
                    .SelectAsLabel().SingleOrDefault()
            });
        }

        public IOperationResult<BlogModel> GetBody(int id, string openKey = "")
        {
            var model = db.Blogs.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<BlogModel>.Fail("数据错误");
            }
            var pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseNetDreamExtensions(this) // 添加自定义代码块渲染器
                    .Build();
            return OperationResult.Ok(new BlogModel()
            {
                Id = model.Id,
                Content = Markdig.Markdown.ToHtml(model.Content, pipeline),
                ClickCount = model.ClickCount,
                RecommendCount = model.RecommendCount,
                CommentCount = model.CommentCount,
            });
        }

        public IOperationResult<BlogModel> OpenBody(int id, string openKey = "")
        {
            return GetBody(id, openKey);
        }

        public BlogArchiveItem[] GetArchives()
        {
            var data = db.Blogs.OrderByDescending(i => i.CreatedAt)
                .Select(i => new BlogListItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                    CreatedAt = i.CreatedAt
                }).ToArray();
            var items = new List<BlogArchiveItem>();
            BlogArchiveItem? last = null;
            foreach (var item in data)
            {
                var date = TimeHelper.TimestampTo(item.CreatedAt);
                if (last != null && last.Year == date.Year)
                {
                    last.Children.Add(item);
                    continue;
                }
                last = new BlogArchiveItem
                {
                    Year = date.Year
                };
                last.Children.Add(item);
                items.Add(last);
            }
            return [..items];
        }

        internal static int PublishCount(BlogContext db, int userId)
        {
            return db.Blogs.Where(i => i.UserId == userId && 
            i.ParentId == 0 && i.PublishStatus == PublishRepository.PUBLISH_STATUS_POSTED)
                .Count();
        }

        public IOperationResult<BlogModel> ManageGet(int id)
        {
            var model = db.Blogs.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<BlogModel>.Fail("数据错误");
            }
            return OperationResult.Ok(new BlogModel()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Content = model.Content,
                ClickCount = model.ClickCount,
                RecommendCount = model.RecommendCount,
                CommentCount = model.CommentCount,
                CreatedAt = model.CreatedAt,
                User = userStore.Get(model.Id),
                Term = db.Categories.Where(i => i.Id == model.TermId)
                    .SelectAsLabel().SingleOrDefault()
            });
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

        public void ManageRemove(int id)
        {
            db.Blogs.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IOperationResult<BlogModel> Recommend(int id)
        {
            var model = db.Blogs.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<BlogModel>.Fail("数据错误");
            }
            var res = Log().ToggleLog(TYPE_BLOG, ACTION_RECOMMEND, id);
            model.RecommendCount += res > 0 ? 1 : -1;
            db.Blogs.Update(model);
            return OperationResult.Ok(new BlogModel()
            {
                Id = id,
                RecommendCount = model.RecommendCount,
                IsLocalization = res > 0
            });
        }

        public ListArticleItem[] Suggest(string keywords)
        {
            return db.Blogs.Search(keywords, "title").Take(4).Select(i => new ListArticleItem()
            {
                Id = i.Id,
                Title = i.Title,
            }).OrderByDescending(i => i.Id).ToArray();
        }
    }
}
