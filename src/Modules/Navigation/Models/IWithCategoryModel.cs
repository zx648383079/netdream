using NetDream.Modules.Navigation.Entities;

namespace NetDream.Modules.Navigation.Models
{
    internal interface IWithCategoryModel
    {
        public int CatId { get; }
        public CategoryEntity? Category { set; }
    }
}