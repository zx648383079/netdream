using NetDream.Modules.OnlineService.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.OnlineService.Models
{
    public class CategoryUserListItem: CategoryUserEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
