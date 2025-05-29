using NetDream.Shared.Models;

namespace NetDream.Modules.Shop.Backend.Models
{
    public interface IWithCategoryModel
    {
        public int CatId { get; }

        public ListLabelItem? Category { set; }
    }
}