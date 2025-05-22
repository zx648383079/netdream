using NetDream.Shared.Models;

namespace NetDream.Modules.Legwork.Models
{
    public interface IWithCategoryModel
    {
        public int CatId { get; set; }

        public ListLabelItem? Category { set; }
    }
}