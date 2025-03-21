using NetDream.Shared.Helpers;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Models;
using System.Collections.Generic;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Entities;
using System.Linq;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Models;

namespace NetDream.Modules.Blog.Repositories
{
    public class BlogRepository(BlogContext db, IUserRepository userStore)
    {
        public TagProvider Tag()
        {
            return new TagProvider(db);
        }
        public IPage<BlogListItem> GetPage(int page)
        {
            var items = db.Blogs.Select<BlogEntity, BlogListItem>().ToPage(page);
            userStore.WithUser(items.Items);
            CategoryRepository.WithCategory(db, items.Items);
            return items;
        }

        

        public IPage<BlogListItem> GetList(QueryForm form)
        {
            SearchHelper.CheckSortOrder(form, ["id", "created_at"]);
            var items = db.Blogs.Search(form.Keywords, "Title")
                .OrderBy<BlogEntity, int>(form.Sort, form.Order)
                .Select<BlogEntity, BlogListItem>()
                .ToPage(form);
            userStore.WithUser(items.Items);
            CategoryRepository.WithCategory(db, items.Items);
            return items;
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

        public BlogModel GetBlog(int id, string? openKey = null)
        {
            return db.Blogs.Where(i => i.Id == id).Single().CopyTo<BlogModel>();
        }

        public List<BlogArchives> GetArchives()
        {
            var data = db.Blogs.OrderByDescending(i => i.CreatedAt)
                .Select(i => new BlogListItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                    CreatedAt = i.CreatedAt
                });
            var items = new List<BlogArchives>();
            BlogArchives? last = null;
            foreach (var item in data)
            {
                var date = TimeHelper.TimestampTo(item.CreatedAt);
                if (last != null && last.Year == date.Year)
                {
                    last.Items.Add(item);
                    continue;
                }
                last = new BlogArchives
                {
                    Year = date.Year
                };
                last.Items.Add(item);
                items.Add(last);
            }
            return items;
        }

   

    }
}
