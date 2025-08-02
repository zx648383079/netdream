using System.Collections.Generic;

namespace NetDream.Modules.Blog.Models
{
    public class BlogArchiveItem
    {
        public int Year { get; set; }

        public List<BlogListItem> Children { get; set; } = [];

    }
}
