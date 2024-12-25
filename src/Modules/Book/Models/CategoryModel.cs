using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Models
{
    public class CategoryModel: CategoryEntity
    {
        public int BookCount { get; set; }

        public string Thumb { get; set; } = string.Empty;
    }
}
