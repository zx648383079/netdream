using NetDream.Modules.OnlineService.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineService.Models
{
    public class CategoryUserModel: CategoryUserEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
