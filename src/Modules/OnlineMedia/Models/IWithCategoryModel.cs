using NetDream.Shared.Models;

namespace NetDream.Modules.OnlineMedia.Models
{
    public interface IWithCategoryModel
    {
        public int CatId { get; }

        public ListLabelItem? Category { set; }
    }
}