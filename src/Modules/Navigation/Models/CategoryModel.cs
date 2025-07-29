using NetDream.Modules.Navigation.Entities;

namespace NetDream.Modules.Navigation.Models
{
    public class CategoryModel : CategoryEntity
    {
        public SiteListItem[] Items { get; internal set; }
    }
}
