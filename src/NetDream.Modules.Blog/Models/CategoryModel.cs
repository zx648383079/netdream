using NetDream.Modules.Blog.Entities;
using NPoco;

namespace NetDream.Modules.Blog.Models
{
    public class CategoryModel: TermEntity
    {
        [Ignore]
        public int BlogCount { get; set; } = 0;
    }
}
