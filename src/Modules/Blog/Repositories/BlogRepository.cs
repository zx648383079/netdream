using NetDream.Shared.Helpers;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Blog.Repositories
{
    public class BlogRepository(IDatabase db)
    {
        public Page<BlogModel> GetPage(int page)
        {
            return db.Page<BlogModel>(page, 20, $"SELECT * FROM {BlogEntity.ND_TABLE_NAME}");
        }

        public List<BlogModel> GetNewBlogs(int count = 8)
        {
            return db.Fetch<BlogModel>($"select id, title, description, created_at from {BlogEntity.ND_TABLE_NAME} order by created_at desc limit @0", count);
        }

        public List<TagModel> GetTags()
        {
            return db.Fetch<TagModel>();
        }

        public List<CategoryModel> Categories()
        {
            return db.Fetch<CategoryModel>();
        }

        public BlogModel GetBlog(int id)
        {
            return db.SingleById<BlogModel>(id);
        }

        public List<BlogArchives> GetArchives()
        {
            var data = db.Fetch<BlogModel>($"select id, title, created_at from {BlogEntity.ND_TABLE_NAME} order by created_at desc");
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
                last = new BlogArchives();
                last.Year = date.Year;
                last.Items.Add(item);
                items.Add(last);
            }
            return items;
        }

    }
}
