using NetDream.Modules.Legwork.Entities;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Legwork.Models
{
    public class ServiceModel : ServiceEntity, IWithUserModel, 
        IWithCategoryModel
    {
        public ListLabelItem? Category { get; set; }
        public IUser? User { get; set; }

        public ProviderModel? Provider { get; set; }
    }
}
