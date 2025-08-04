using NetDream.Modules.OnlineService.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineService.Models
{
    public class SessionListItem: SessionEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
