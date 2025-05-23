using NetDream.Modules.ResourceStore.Entities;

namespace NetDream.Modules.ResourceStore.Models
{
    public class CategoryModel : CategoryEntity
    {
        public CategoryEntity[] Children { get; set; }

        public ResourceListItem[] Items { get; set; }
    }
}
