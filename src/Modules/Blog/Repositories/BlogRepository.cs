using NetDream.Shared.Helpers;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Models;
using System.Collections.Generic;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Entities;
using System.Linq;
using NetDream.Shared.Models;

namespace NetDream.Modules.Blog.Repositories
{
    public class BlogRepository(BlogContext db)
    {
        public IPage<BlogModel> GetPage(int page)
        {
            return db.Blogs.ToPage(page).CopyTo<BlogEntity, BlogModel>();
        }

        public IPage<BlogModel> GetList(QueryForm form)
        {
            SearchHelper.CheckSortOrder(form, ["id", "created_at"]);
            return db.Blogs.Search(form.Keywords, "Title")
                .OrderBy<BlogEntity, int>(form.Sort, form.Order)
                .ToPage(form).CopyTo<BlogEntity, BlogModel>();
        }

        public BlogEntity[] GetNewBlogs(int count = 8)
        {
            return db.Blogs.OrderByDescending(i => i.CreatedAt)
                .Select(i => new BlogEntity()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    CreatedAt = i.CreatedAt
                }).Take(count).ToArray();
        }

        public TagModel[] GetTags()
        {
            return db.Tags.Select<TagEntity, TagModel>().ToArray();
        }

        public CategoryModel[] Categories()
        {
            return db.Categories.Select<CategoryEntity, CategoryModel>().ToArray();
        }

        public BlogModel GetBlog(int id)
        {
            return db.Blogs.Where(i => i.Id == id).Single().CopyTo<BlogModel>();
        }

        public List<BlogArchives> GetArchives()
        {
            var data = db.Blogs.OrderByDescending(i => i.CreatedAt)
                .Select(i => new BlogEntity()
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
