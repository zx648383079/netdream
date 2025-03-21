using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Blog.Models
{
    internal interface IWithCategoryModel
    {
        public int TermId { get; }

        public IListLabelItem? Term { get; set; }
    }
}
