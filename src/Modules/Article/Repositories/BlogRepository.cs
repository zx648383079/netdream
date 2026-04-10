using Markdig;
using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Forms;
using NetDream.Modules.Article.Models;
using NetDream.Modules.Blog.Markdown;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Article.Repositories
{
    public class BlogRepository(
        ArticleContext db, 
        IClientContext client,
        IUserRepository userStore, 
        ITagRepository tagStore,
        IInteractRepository interactStore,
        ICategoryRepository categoryStore,
        IDeeplink deeplink)
    {
        public const byte TYPE_BLOG = 0;
        public const byte TYPE_COMMENT = 1;

        public const byte ACTION_RECOMMEND = 0;
        public const byte ACTION_AGREE = 1;
        public const byte ACTION_DISAGREE = 2;

        public const byte ACTION_REAL_RULE = 3; // 是否能阅读

        public IPage<ArticleListItem> SelfList(ArticleQueryForm form)
        {
            form.User = client.UserId;
            return GetList(form);
        }

        private static void CheckSortOrder(QueryForm form)
        {
            switch (form.Sort)
            {
                case "new":
                    form.Sort = "created_at";
                    form.Order = "desc";
                    break;
                case "recommend":
                case "best":
                    form.Sort = "recommend_count'";
                    form.Order = "desc";
                    break;
                case "hot":
                    form.Sort = "comment_count";
                    form.Order = "desc";
                    break;
                default:
                    SearchHelper.CheckSortOrder(form, ["id", "created_at"]);
                    break;
            }
        }

        public IPage<ArticleListItem> GetList(ArticleQueryForm form)
        {
            CheckSortOrder(form);
            var include = Array.Empty<int>();
            if (!string.IsNullOrWhiteSpace(form.Tag))
            {
                include = tagStore.Get(ModuleTargetType.Article, form.Tag);
                if (include.Length == 0)
                {
                    return new Page<ArticleListItem>();
                }
            }
            var items = db.Articles.Search(form.Keywords, "Title")
                .Where(i => i.PublishStatus == (byte)PublishStatus.Posted
                && i.Status == (byte)ReviewStatus.Approved)
                .When(form.Category > 0, i => i.CatId == form.Category)
                .When(form.User > 0, i => i.UserId == form.User)
                .When(!string.IsNullOrWhiteSpace(form.Language), i => i.Language == form.Language, i => i.ParentId == 0)
                //.When(form.ProgrammingLanguage, i => i.ProgrammingLanguage == form.ProgrammingLanguage)
                .When(include.Length > 0, i => include.Contains(i.Id))
                .OrderBy(form)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            categoryStore.Include(ModuleTargetType.Article, items.Items);
            return items;
        }

        public IPage<ArticleListItem> AdvancedList(ArticleQueryForm form)
        {
            CheckSortOrder(form);
            var include = Array.Empty<int>();
            if (!string.IsNullOrWhiteSpace(form.Tag))
            {
                include = tagStore.Get(ModuleTargetType.Article, form.Tag);
                if (include.Length == 0)
                {
                    return new Page<ArticleListItem>();
                }
            }
            var items = db.Articles.Search(form.Keywords, "Title")
                .Where(i => i.PublishStatus == (byte)PublishStatus.Posted
                && i.Status == (byte)ReviewStatus.Approved)
                .When(form.Category > 0, i => i.CatId == form.Category)
                .When(form.User > 0, i => i.UserId == form.User)
                .When(!string.IsNullOrWhiteSpace(form.Language), i => i.Language == form.Language, i => i.ParentId == 0)
                // .When(form.ProgrammingLanguage, i => i.ProgrammingLanguage == form.ProgrammingLanguage)
                .When(include.Length > 0, i => include.Contains(i.Id))
                .OrderBy(form)
                .OrderBy(form)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            categoryStore.Include(ModuleTargetType.Article, items.Items);
            return items;
        }

        public ArticleListItem[] GetList(int[] items)
        {
            if (items.Length == 0)
            {
                return [];
            }
            var data = db.Articles.Where(i => items.Contains(i.Id) && i.PublishStatus == (byte)PublishStatus.Posted
                && i.Status == (byte)ReviewStatus.Approved)
                .SelectAsLabel().ToArray();
            foreach (var item in data)
            {
                item.Url = deeplink.Encode($"blog/{item.Id}");
            }
            return data;
        }

        public ArticleListItem[] GetNewBlogs(int count = 8)
        {
            return db.Articles.Where(i => i.PublishStatus == (byte)PublishStatus.Posted
                && i.Status == (byte)ReviewStatus.Approved).OrderByDescending(i => i.CreatedAt)
                .SelectAsLabel().Take(count).ToArray();
        }

        public ArticleListItem[] GetRelationBlogs(int sourceId)
        {
            var items = tagStore.GetRelation(ModuleTargetType.Article, sourceId);
            if (items.Length == 0)
            {
                return [];
            }
            return db.Articles.Where(i => items.Contains(i.Id) &&
                i.PublishStatus == (byte)PublishStatus.Posted
                && i.Status == (byte)ReviewStatus.Approved)
                .OrderByDescending(i => i.CreatedAt)
                .Take(5)
                .SelectAsLabel().ToArray();
        }

        public StatisticsItem[] GetTags()
        {
            return tagStore.Get(ModuleTargetType.Article);
        }

        public IListStatisticsItem[] Categories()
        {
            var res = db.Categories.Where(i => i.Type == (byte)ModuleTargetType.Article)
                .Select(i => new ListStatisticsItem(i.Id, i.Name))
                .ToArray();
            var items = db.Articles.Where(i => i.Type == (byte)ModuleTargetType.Article 
                && i.PublishStatus == (byte)PublishStatus.Posted)
                .GroupBy(i => i.CatId)
                .Select(i => new KeyValuePair<int, int>(i.Key, i.Count()))
                .ToDictionary();
            foreach (var item in res)
            {
                if (items.TryGetValue(item.Id, out var c))
                {
                    item.Count = c;
                }
            }
            return res;
        }

        /// <summary>
        /// 前台获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="openKey"></param>
        /// <returns></returns>
        public IOperationResult<ArticleOpenModel> Get(int id, string openKey = "")
        {
            if (id <= 0)
            {
                return OperationResult<ArticleOpenModel>.Fail("数据错误");
            }
            var model = db.Articles.Where(i => i.Id == id &&
                i.PublishStatus == (byte)PublishStatus.Posted
                && i.Status == (byte)ReviewStatus.Approved).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ArticleOpenModel>.Fail("数据错误");
            }
            var pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseNetDreamExtensions(this) // 添加自定义代码块渲染器
                    .Build();
            return OperationResult.Ok(new ArticleOpenModel()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Content = Markdown.ToHtml(model.Content, pipeline),
                ClickCount = model.ClickCount,
                LikeCount = model.LikeCount,
                CommentCount = model.CommentCount,
                CreatedAt = model.CreatedAt,
                User = userStore.Get(model.Id),
                Category = db.Categories.Where(i => i.Id == model.CatId)
                    .SelectAsLabel().SingleOrDefault()
            });
        }

        public IOperationResult<ArticleOpenModel> GetBody(int id, string openKey = "")
        {
            var model = db.Articles.Where(i => i.Id == id &&
                i.PublishStatus == (byte)PublishStatus.Posted
                && i.Status == (byte)ReviewStatus.Approved).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ArticleOpenModel>.Fail("数据错误");
            }
            var pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseNetDreamExtensions(this) // 添加自定义代码块渲染器
                    .Build();
            return OperationResult.Ok(new ArticleOpenModel()
            {
                Id = model.Id,
                Content = Markdown.ToHtml(model.Content, pipeline),
                ClickCount = model.ClickCount,
                LikeCount = model.LikeCount,
                CommentCount = model.CommentCount,
            });
        }

        public IOperationResult<ArticleOpenModel> OpenBody(int id, string openKey = "")
        {
            return GetBody(id, openKey);
        }

        public ArchiveListItem[] GetArchives()
        {
            var data = db.Articles.Where(i =>
                i.PublishStatus == (byte)PublishStatus.Posted
                && i.Status == (byte)ReviewStatus.Approved)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => new ArchiveLabelItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                    CreatedAt = i.CreatedAt
                }).ToArray();
            var items = new List<ArchiveListItem>();
            ArchiveListItem? last = null;
            foreach (var item in data)
            {
                var date = TimeHelper.TimestampTo(item.CreatedAt);
                if (last != null && last.Year == date.Year)
                {
                    last.Children.Add(item);
                    continue;
                }
                last = new ArchiveListItem
                {
                    Year = date.Year
                };
                last.Children.Add(item);
                items.Add(last);
            }
            return [..items];
        }

        internal static int PublishCount(ArticleContext db, int userId)
        {
            return db.Articles.Where(i => i.UserId == userId && 
            i.ParentId == 0 && i.PublishStatus == (byte)PublishStatus.Posted)
                .Count();
        }

        public IOperationResult<ArticleModel> AdvancedGet(int id)
        {
            var model = db.Articles.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<ArticleModel>.Fail("数据错误");
            }
            return OperationResult.Ok(new ArticleModel()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Content = model.Content,
                ClickCount = model.ClickCount,
                LikeCount = model.LikeCount,
                CommentCount = model.CommentCount,
                CreatedAt = model.CreatedAt,
                User = userStore.Get(model.Id),
                Category = db.Categories.Where(i => i.Id == model.CatId)
                    .SelectAsLabel().SingleOrDefault()
            });
        }

        public IOperationResult<ArticleEntity> AdvancedChange(int id, byte status)
        {
            var model = db.Articles.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ArticleEntity>.Fail("id is error");
            }
            model.Status = status;
            db.Update(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void AdvancedRemove(int id)
        {
            db.Articles.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IOperationResult<ArticleOpenModel> Recommend(int id)
        {
            var model = db.Articles.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<ArticleOpenModel>.Fail("数据错误");
            }
            var res = interactStore.Toggle(client.UserId, 
                ModuleTargetType.Article, id, InteractType.Like);
            model.LikeCount += res ? 1 : -1;
            db.Articles.Update(model);
            return OperationResult.Ok(new ArticleOpenModel()
            {
                Id = id,
                LikeCount = model.LikeCount,
                IsLiked = res
            });
        }

        public IListArticleItem[] Suggest(string keywords)
        {
            return db.Articles.Search(keywords, "title").Take(4).Select(i => new ListArticleItem()
            {
                Id = i.Id,
                Title = i.Title,
            }).OrderByDescending(i => i.Id).ToArray();
        }
    }
}
