using Microsoft.AspNetCore.Identity;
using NetDream.Areas.Blog.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Blog.Repositories
{
    public class BlogRepository
    {
        private readonly IDatabase _db;

        public BlogRepository(IDatabase db)
        {
            _db = db;
        }


        public List<BlogModel> GetNewBlogs()
        {
            return _db.Fetch<BlogModel>("select id, title, description, created_at from blog created_at desc limit 8");
        }
    }
}
