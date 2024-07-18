using NetDream.Modules.Blog.Entities;
using NPoco;

namespace NetDream.Modules.Blog.Models
{
    public class CategoryModel: CategoryEntity
    {
        [Ignore]
        public int BlogCount { get; set; } = 0;
    }
}
