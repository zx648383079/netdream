using NetDream.Modules.Navigation.Entities;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Modules.Navigation.Models
{
    public class SiteModel : SiteEntity, IWithUserModel, IWithCategoryModel
    {

        public CategoryEntity? Category { get; set; }
        public IUser? User { get; set; }

        public TagEntity[] Tags { get; set; }
    }
}
