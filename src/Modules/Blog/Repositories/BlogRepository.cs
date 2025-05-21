using Markdig;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Markdown;
using NetDream.Modules.Blog.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Blog.Repositories
{
    public class BlogRepository(
        BlogContext db, 
        IUserRepository userStore, 
        IDeeplink deeplink)
    {
        public TagProvider Tag()
        {
            return new TagProvider(db);
        }
        public IPage<BlogListItem> GetPage(int page)
        {
            var items = db.Blogs.Select<BlogEntity, BlogListItem>().ToPage(page);
            userStore.Include(items.Items);
            CategoryRepository.Include(db, items.Items);
            return items;
        }

        

        public IPage<BlogListItem> GetList(QueryForm form)
        {
            SearchHelper.CheckSortOrder(form, ["id", "created_at"]);
            var items = db.Blogs.Search(form.Keywords, "Title")
                .OrderBy<BlogEntity, int>(form.Sort, form.Order)
                .Select<BlogEntity, BlogListItem>()
                .ToPage(form);
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
                .Select(i => new BlogListItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                    CreatedAt = i.CreatedAt,
                    Description = i.Description,
                }).ToArray();
            foreach (var item in data)
            {
                item.Url = deeplink.Encode($"blog/{item.Id}");
            }
            return data;
        }

        public BlogListItem[] GetNewBlogs(int count = 8)
        {
            return db.Blogs.OrderByDescending(i => i.CreatedAt)
                .Select(i => new BlogListItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    CreatedAt = i.CreatedAt
                }).Take(count).ToArray();
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
                .Select(i => new BlogListItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    CreatedAt = i.CreatedAt
                }).ToArray();
        }

        public TagListItem[] GetTags()
        {
            return db.Tags.Select<TagEntity, TagListItem>().ToArray();
        }

        public CategoryListItem[] Categories()
        {
            return db.Categories.Select<CategoryEntity, CategoryListItem>().ToArray();
        }

        public BlogModel? GetBlog(int id, string? openKey = null)
        {
            if (id <= 0)
            {
                return null;
            }
            var model = db.Blogs.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return null;
            }
            var pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseNetDreamExtensions(this) // 添加自定义代码块渲染器
                    .Build();
            return new BlogModel()
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
                    .Select(i => new CategoryListItem()
                    {
                        Id = i.Id,
                        Name = i.Name,
                    }).SingleOrDefault()
            };
        }

        public BlogArchives[] GetArchives()
        {
            var data = db.Blogs.OrderByDescending(i => i.CreatedAt)
                .Select(i => new BlogListItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                    CreatedAt = i.CreatedAt
                }).ToArray();
            var items = new List<BlogArchives>();
            BlogArchives? last = null;
            foreach (var item in data)
            {
                var date = TimeHelper.TimestampTo(item.CreatedAt);
                if (last != null && last.Year == date.Year)
                {
                    last.Children.Add(item);
                    continue;
                }
                last = new BlogArchives
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
    }
}
