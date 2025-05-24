using NetDream.Shared.Models;

namespace NetDream.Modules.Document.Models
{
    public interface IWithCategoryModel
    {
        public int CatId { get; }

        public ListLabelItem? Category { set; } 
    }
}