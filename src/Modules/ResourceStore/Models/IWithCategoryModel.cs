using NetDream.Shared.Models;

namespace NetDream.Modules.ResourceStore.Models
{
    public interface IWithCategoryModel
    {
        public int CatId { get; }

        public ListLabelItem? Category { set; } 
    }
}