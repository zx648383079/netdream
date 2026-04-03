using NetDream.Modules.Navigation.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Navigation.Models
{
    public class SiteModel : SiteEntity, IWithUserModel, IWithCategoryModel
    {

        public IListLabelItem? Category { get; set; }
        public IUser? User { get; set; }

        public string[] Tags { get; set; }
    }
}
