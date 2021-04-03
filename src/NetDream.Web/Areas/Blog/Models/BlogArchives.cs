using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Blog.Models
{
    public class BlogArchives
    {
        public int Year { get; set; }

        public List<BlogModel> Items = new();

    }
}
