using NetDream.Core.Helpers;
using NetDream.Web.Areas.Blog.Models;
using NPoco;
using System.Collections.Generic;

namespace NetDream.Web.Areas.Blog.Repositories
{
    public class BlogRepository
    {
        private readonly IDatabase _db;

        public BlogRepository(IDatabase db)
        {
            _db = db;
        }


        public Page<BlogModel> GetPage(int page)
        {
            return _db.Page<BlogModel>(page, 20, "SELECT * FROM blog");
        }

        public List<BlogModel> GetNewBlogs(int count = 8)
        {
            return _db.Fetch<BlogModel>("select id, title, description, created_at from blog order by created_at desc limit @0", count);
        }

        public List<TagModel> GetTags()
        {
            return _db.Fetch<TagModel>();
        }

        public List<CategoryModel> Categories()
        {
            return _db.Fetch<CategoryModel>();
        }

        public BlogModel GetBlog(int id)
        {
            return _db.SingleById<BlogModel>(id);
        }
            
        public List<BlogArchives> GetArchives()
        {
            var data = _db.Fetch<BlogModel>("select id, title, created_at from blog order by created_at desc");
            var items = new List<BlogArchives>();
            BlogArchives last = null;
            foreach (var item in data)
            {
                var date = Time.TimestampTo(item.CreatedAt);
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
