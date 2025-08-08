using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Auth.Models
{
    public class InviteLogListItem : InviteLogEntity
    {
        public IUser? User { get; set; }
        public IUser? Inviter { get; set; }
    }
}
