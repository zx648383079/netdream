using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Blog.Models
{
    public class CategoryLabelItem : IListLabelItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int BlogCount { get; set; }
    }
}