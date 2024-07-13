using Modules.OnlineService.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;

namespace NetDream.Modules.OnlineService.Models
{
    public class CategoryUserModel: CategoryUserEntity, IWithUserModel, IWithCategoryModel
    {
        [Ignore]
        public IUser? User { get; set; }
        [Ignore]
        public CategoryEntity? Category { get; set; }
    }
}
