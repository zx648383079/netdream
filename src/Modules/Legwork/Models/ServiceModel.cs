using NetDream.Modules.Legwork.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Legwork.Models
{
    public class ServiceModel : ServiceEntity, IWithUserModel, 
        IWithCategoryModel
    {
        public IListLabelItem? Category { get; set; }
        public IUser? User { get; set; }

        public ProviderModel? Provider { get; set; }
    }
}
