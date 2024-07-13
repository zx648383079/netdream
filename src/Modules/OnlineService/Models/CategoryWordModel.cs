using Modules.OnlineService.Entities;
using NPoco;

namespace NetDream.Modules.OnlineService.Models
{
    public class CategoryWordModel: CategoryWordEntity, IWithCategoryModel
    {
        [Ignore]
        public CategoryEntity? Category { get; set; }
    }
}
