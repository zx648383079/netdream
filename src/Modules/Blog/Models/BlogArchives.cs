using System.Collections.Generic;

namespace NetDream.Modules.Blog.Models
{
    public class BlogArchives
    {
        public int Year { get; set; }

        public List<BlogListItem> Items = [];

    }
}
