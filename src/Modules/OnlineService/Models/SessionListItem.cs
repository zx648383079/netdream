using NetDream.Modules.OnlineService.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.OnlineService.Models
{
    public class SessionListItem: SessionEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
