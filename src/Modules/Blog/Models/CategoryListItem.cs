using NetDream.Modules.Blog.Entities;

namespace NetDream.Modules.Blog.Models
{
    public class CategoryListItem: CategoryEntity
    {
        public int BlogCount { get; set; } = 0;
    }
}
