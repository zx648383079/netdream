using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Models
{
    public class CategoryListItem: CategoryEntity
    {
        public int BookCount { get; set; }

        public string Thumb { get; set; } = string.Empty;
    }
}
